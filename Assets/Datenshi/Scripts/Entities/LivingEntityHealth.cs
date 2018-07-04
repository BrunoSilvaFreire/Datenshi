using Datenshi.Scripts.Combat;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity {
        [TitleGroup(CombatGroup, "Informações sobre o combat desta LivingEntity")]
        public EntityDamagedEvent OnDamaged;

        [TitleGroup(CombatGroup)]
        public UnityEvent OnHealthChanged;

        [SerializeField]
        private UnityEvent onKilled;

        public UnityEvent OnKilled => onKilled;

        [SerializeField, HideInInspector]
        private uint health;

        [SerializeField, HideInInspector]
        private uint maxHealth;

        [SerializeField, HideInInspector]
        private bool godMode;


        [ShowIf(nameof(HasTemporaryInvulnerability)), ReadOnly, TitleGroup(HealthGroup)]
        private float invulnerabilitySecondsLeft;

        public float DamageColorDuration;

        public Color DamageColor;

        public float DamageColorAmount;

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

        [ShowInInspector, TitleGroup(HealthGroup)]
        public bool GodMode {
            get {
                return godMode;
            }
            set {
                godMode = value;
            }
        }


        public bool IsInvulnerable => GodMode || HasTemporaryInvulnerability;


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

        public virtual uint Damage(ICombatant entity, ref DamageInfo info, IDefendable defendable = null) {
            if (defendable != null && Defending && defendable.CanAutoDefend(this)) {
                defendable.AutoDefend(this, ref info);
                return 0;
            }

            if (IsInvulnerable || Ignored || Dead || info.Canceled) {
                return 0;
            }

            var attack = info.Attack;
            var multiplier = info.Multiplier;

            var damage = (uint) (attack.GetDamage(this) * multiplier);
            GlobalEntityDamagedEvent.Instance.Invoke(this, entity, attack, damage);
            if (damage >= health) {
                Debug.Log($"<color=#FF0000><b>{name} killed by {entity} @ {damage}</b></color>");
                Kill();
                return health;
            }

            Debug.Log($"<color=#FF0000>{name} damaged by {entity} @ {damage}</color>");

            OnDamaged.Invoke(entity, damage);
            OnHealthChanged.Invoke();
            ColorizableRenderer.RequestColorOverride(DamageColor, DamageColorAmount, DamageColorDuration);
            Health -= damage;
            if (DamageInvulnerability) {
                SetInvulnerable(DamageInvulnerabilityDuration);
            }

            if (DamageGivesStun && damage >= DamageStunMin) {
                Stun(DamageStunDuration);
            }

            return damage;
        }

        [ShowInInspector, TitleGroup(HealthGroup)]
        public bool HasTemporaryInvulnerability => invulnerabilitySecondsLeft > 0;


        public void SetInvulnerable(float seconds) {
            invulnerabilitySecondsLeft += seconds;
        }

        private void UpdateInvulnerability() {
            if (invulnerabilitySecondsLeft > 0) {
                invulnerabilitySecondsLeft -= Time.deltaTime;
            }
        }

        public void Kill() {
            if (Dead) {
                return;
            }

            OnHealthChanged.Invoke();
            health = 0;
            onKilled.Invoke();
            updater.TriggerDeath();
        }

        public void Heal(uint healthAmount) {
            Health += healthAmount;
            OnHealthChanged.Invoke();
        }
    }
}