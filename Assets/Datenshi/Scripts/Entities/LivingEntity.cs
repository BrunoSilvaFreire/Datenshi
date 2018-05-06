using System;
using System.Collections;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UPM.Motors;
using UPM.Util;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Datenshi.Scripts.Entities {
    [Serializable]
    public class EntityDamagedEvent : UnityEvent<ICombatant, uint> { }

    [Serializable]
    public class EntityAttackEvent : UnityEvent<Attack> { }

    /// <summary>
    /// Uma entidade que tem vida, vida máxima, e pode ser morta.
    /// </summary>
    public class LivingEntity : Entity, ICombatant {
        public const string HealthGroup = "Health";
        public const string CombatGroup = "Combat";


        [SerializeField, HideInInspector]
        private uint health;

        [SerializeField, HideInInspector]
        private uint maxHealth;

        [SerializeField, HideInInspector]
        private bool invulnerable;

        [TitleGroup(HealthGroup, "Informações sobre a vida desta LivingEntity")]
        public bool DamageInvulnerability;

        [ShowIf("DamageInvulnerability")]
        public float DamageInvulnerabilityDuration = 3;

        [TitleGroup(CombatGroup, "Informações sobre o combat desta LivingEntity"), SerializeField]
        private CombatRelationship relationship;

        [TitleGroup(CombatGroup)]
        public EntityDamagedEvent OnDamaged;

        [TitleGroup(CombatGroup)]
        public EntityAttackEvent OnAttack;

        [TitleGroup(CombatGroup)]
        public UnityEvent OnHealthChanged;

        [TitleGroup(CombatGroup)]
        public bool DamageGivesStun;

        [SerializeField]
        private Collider2D hitbox;

        public Collider2D Hitbox {
            get {
                return hitbox;
            }
        }

        public bool CanFocus {
            get {
                return FocusTimeLeft > MinDefenseRequired;
            }
        }

        [TitleGroup(CombatGroup)]
        public bool Stunned {
            get;
            private set;
        }

        [ShowIf("DamageGivesStun"), TitleGroup(CombatGroup)]
        public uint DamageStunMin = 10;

        [ShowIf("DamageGivesStun"), TitleGroup(CombatGroup)]
        public float DamageStunDuration = 1;

        public UnityEvent OnKilled;

        [ShowIf("DamageGivesStun"), TitleGroup(CombatGroup), ReadOnly]
        private float totalStunTimeLeft;

        [SerializeField]
        private Bounds2D defenseHitbox;

        public float FocusMaxTime = 2;

        private bool focusing;

        public float MinDefenseRequired = 0.1F;
        public float DefenseRecoverAmountMultiplier = 1;
        public float DefenseDepleteAmountMultiplier = 2;

        [SerializeField]
        private CombatantAnimatorUpdater updater;

        public CombatantAnimatorUpdater AnimatorUpdater {
            get {
                return updater;
            }
        }

        public float FocusTimeLeft {
            get;
            set;
        }

        public bool Focusing {
            get {
                return focusing;
            }
            set {
                if (focusing == value) {
                    return;
                }

                if (value && !CanFocus) {
                    return;
                }

                focusing = value;
            }
        }

        private void Update() {
            if (Stunned) {
                totalStunTimeLeft -= Time.deltaTime;
                if (totalStunTimeLeft < 0) {
                    Stunned = false;
                }
            }

            if (focusing) {
                if (FocusTimeLeft <= 0) {
                    focusing = false;
                } else {
                    FocusTimeLeft -= Time.deltaTime * DefenseDepleteAmountMultiplier;
                }
            } else {
                var recoverAmount = Time.deltaTime * DefenseRecoverAmountMultiplier;
                if (FocusTimeLeft + recoverAmount > FocusMaxTime) {
                    FocusTimeLeft = FocusMaxTime;
                } else {
                    FocusTimeLeft += recoverAmount;
                }
            }

            if (updater == null) {
                return;
            }

            updater.SetDefend(focusing);
        }

        public void Stun(float duration) {
            if (Invulnerable) {
                return;
            }

            Stunned = true;
            totalStunTimeLeft += duration;
        }

        /// <summary>
        /// O total de vida da entidade atual.
        /// <br/>
        /// Se for setado para 0, o método <see cref="Kill"/> será chamado.
        /// <br/>
        /// Se for setado para um valor maior que <see cref="MaxHealth"/>, <see cref="Health"/> será setado 
        /// igual a <see cref="MaxHealth"/>. 
        /// </summary>
        [ShowInInspector, TitleGroup(HealthGroup, "Informações sobre a vida desta LivingEntity")]
        public uint Health {
            get {
                return health;
            }
            private set {
                if (value == 0) {
#if UNITY_EDITOR
                    if (EditorApplication.isPlaying) {
#endif
                        Kill();
                        return;
#if UNITY_EDITOR
                    }
#endif
                }


                health = value > maxHealth ? maxHealth : value;
                OnHealthChanged.Invoke();
            }
        }

        public void ExecuteAttack(Attack attack) {
            if (Stunned || attack == null) {
                return;
            }

            OnAttack.Invoke(attack);
            attack.Execute(this);
        }

        /// <summary>
        /// A vida máxima da entidade.
        /// <br/>
        /// Se for setada para um valor menor que <see cref="Health"/>, <see cref="Health"/> será setado 
        /// igual a <see cref="MaxHealth"/>
        /// </summary>
        [ShowInInspector, TitleGroup(HealthGroup)]
        public uint MaxHealth {
            get {
                return maxHealth;
            }
            set {
                maxHealth = value;
                if (health > value) {
                    health = value;
                }
            }
        }

        /// <summary>
        /// A porcentagem de vida atual em uma escala de 0 -> 1.
        /// <br/>
        /// Qualquer tentativa de mudar este valor delega a chamada para <see cref="Health"/>. 
        /// </summary>
        [ShowInInspector, TitleGroup(HealthGroup), ProgressBar(0, 1), PropertyRange(0, 1)]
        public float HealthPercentage {
            get {
                return (float) Health / MaxHealth;
            }
            set {
                Health = (uint) (value * MaxHealth);
            }
        }

        [SerializeField]
        private bool ignored;

        [ShowInInspector, TitleGroup(HealthGroup)]
        public bool Invulnerable {
            get {
                return invulnerable;
            }
            set {
                invulnerable = value;
            }
        }

        [ShowIf("HasTemporaryInvulnerability"), ReadOnly,
         TitleGroup(HealthGroup)]
        private float invulnerabilitySecondsLeft;

        [ShowInInspector, TitleGroup(HealthGroup)]
        public bool HasTemporaryInvulnerability {
            get {
                return invulnerabilityCoroutine != null;
            }
        }

        private Coroutine invulnerabilityCoroutine;

        public void SetInvulnerable(float seconds) {
            invulnerabilitySecondsLeft += seconds;
            if (invulnerabilityCoroutine == null) {
                invulnerabilityCoroutine = StartCoroutine(InvulnerabilityCoroutine());
            }
        }

        private IEnumerator InvulnerabilityCoroutine() {
            invulnerable = true;
            while (invulnerabilitySecondsLeft >= 0) {
                invulnerabilitySecondsLeft -= Time.deltaTime;
                yield return null;
            }

            invulnerabilityCoroutine = null;
            Invulnerable = false;
        }

        public bool IsEnemy(LivingEntity entity) {
            if (Relationship == CombatRelationship.Neutral || entity.Relationship == CombatRelationship.Neutral) {
                return false;
            }

            return Relationship != entity.Relationship;
        }

        public bool IsNeutral {
            get {
                return Relationship == CombatRelationship.Neutral;
            }
        }

        public bool IsAlly {
            get {
                return Relationship == CombatRelationship.Ally;
            }
        }

        public float FocusTimePercent {
            get {
                return FocusTimeLeft / FocusMaxTime;
            }
        }

        public void Kill() {
            //TODO: Delegar efeitos de morte para um outro objeto
            health = 0;
            OnKilled.Invoke();
            Destroy(gameObject);
        }

        public void Heal(uint healthAmount) {
            Health += healthAmount;
        }

        public virtual void Damage(ICombatant entity, uint damage) {
            if (Invulnerable || Ignored) {
                return;
            }

            Debug.Log(name + " damaged by " + entity + " @ " + damage);
            if (damage >= health) {
                Kill();
                return;
            }

            OnDamaged.Invoke(entity, damage);
            Health -= damage;
            if (DamageInvulnerability) {
                SetInvulnerable(DamageInvulnerabilityDuration);
            }

            if (DamageGivesStun && damage >= DamageStunMin) {
                Stun(DamageStunDuration);
            }
        }

        public Direction CurrentDirection {
            get {
                return direction;
            }
            set {
                direction = value;
            }
        }

        public GameObject GameObject {
            get {
                return gameObject;
            }
        }

        public bool Ignored {
            get {
                return ignored;
            }
            set {
                ignored = value;
            }
        }

        public Bounds2D DefenseHitbox {
            get {
                var hb = defenseHitbox;
                hb.Center.x *= CurrentDirection.X;
                hb.Center += (Vector2) transform.position;
                return hb;
            }
        }

        public CombatRelationship Relationship {
            get {
                return relationship;
            }
        }

        public Transform Transform {
            get {
                return transform;
            }
        }


        public Vector2 Center {
            get {
                var m = this as IMovable;
                if (m != null) {
                    var hb = m.Hitbox;
                    return hb != null ? hb.bounds.center : transform.position;
                }

                return transform.position;
            }
        }


        public Vector2 GroundPosition {
            get {
                var pos = Center;
                pos.y -= hitbox.bounds.size.y / 2;
                return pos;
            }
        }

        private void OnDrawGizmosSelected() {
            DebugUtil.DrawBounds2D(DefenseHitbox, Color.magenta);
        }
    }
}