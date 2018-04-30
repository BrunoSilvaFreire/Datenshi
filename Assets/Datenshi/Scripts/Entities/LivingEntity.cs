using System;
using System.Collections;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Combat.Strategies;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Datenshi.Scripts.Entities {
    public enum EntityRelationship {
        Ally,
        Neutral,
        Enemy
    }

    [Serializable]
    public class EntityDamagedEvent : UnityEvent<LivingEntity, uint> { }

    [Serializable]
    public class EntityAttackEvent : UnityEvent<Attack> { }

    /// <summary>
    /// Uma entidade que tem vida, vida máxima, e pode ser morta.
    /// </summary>
    public class LivingEntity : Entity {
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

        [TitleGroup(CombatGroup, "Informações sobre o combat desta LivingEntity")]
        public EntityRelationship Relationship;

        [TitleGroup(CombatGroup, "Informações sobre o combat desta LivingEntity")]
        public AttackStrategy DefaultAttackStrategy;

        [TitleGroup(CombatGroup)]
        public EntityDamagedEvent OnDamaged;

        [TitleGroup(CombatGroup)]
        public EntityAttackEvent OnAttack;

        [TitleGroup(CombatGroup)]
        public UnityEvent OnHealthChanged;

        [TitleGroup(CombatGroup)]
        public bool DamageGivesStun;

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

        public Bounds2D DefaultAttackHitbox;
        public float FocusMaxTime = 2;
        private bool defending;
        public float MinDefenseRequired = 0.1F;
        public float DefenseRecoverAmountMultiplier = 1;
        public float DefenseDepleteAmountMultiplier = 2;

        public float FocusTimeLeft {
            get;
            set;
        }

        public bool Defending {
            get {
                return defending;
            }
            set {
                if (Defending == value) {
                    return;
                }

                if (value && !CanDefend) {
                    return;
                }

                defending = value;
            }
        }

        public bool CanDefend {
            get {
                return FocusTimeLeft > MinDefenseRequired;
            }
        }

        private void Update() {
            if (Stunned) {
                totalStunTimeLeft -= Time.deltaTime;
                if (totalStunTimeLeft < 0) {
                    Stunned = false;
                }
            }

            if (Defending) {
                if (FocusTimeLeft <= 0) {
                    Defending = false;
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

            var updater = AnimatorUpdater;
            if (updater == null) {
                return;
            }

            updater.SetDefend(Defending);
            if (InputProvider.GetAttack()) {
                updater.TriggerAttack();
            }
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
            if (Stunned) {
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
        [ShowInInspector, TitleGroup(HealthGroup)]
        public float HealthPercentage {
            get {
                return (float) Health / MaxHealth;
            }
            set {
                Health = (uint) (value * MaxHealth);
            }
        }

        public bool Ignored;

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
            if (Relationship == EntityRelationship.Neutral || entity.Relationship == EntityRelationship.Neutral) {
                return false;
            }

            return Relationship != entity.Relationship;
        }

        public bool IsNeutral {
            get {
                return Relationship == EntityRelationship.Neutral;
            }
        }

        public bool IsAlly {
            get {
                return Relationship == EntityRelationship.Ally;
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

        public virtual void Damage(LivingEntity entity, uint damage) {
            if (Invulnerable || Ignored) {
                return;
            }

            Debug.Log(name + " damaged by " + entity.name + " @ " + damage);
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

        public bool ShouldAttack(LivingEntity en) {
            return en != null && !en.Ignored && Relationship != en.Relationship;
        }

        private void Reset() {
            DefaultAttackStrategy = EmptyStrategy.Instance;
        }
    }
}