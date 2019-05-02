using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Graphics.Colorization;
using Shiroi.FX.Utilities;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity {
        [SerializeField, BoxGroup(CombatGroup)]
        private CombatantDamagedEvent onDamaged;

        [BoxGroup(CombatGroup)]
        public UnityEvent OnHealthChanged;

        [SerializeField, BoxGroup(CombatGroup)]
        private UnityEvent onKilled;

        public UnityEvent OnKilled => onKilled;

        [SerializeField, HideInInspector]
        private uint health;

        [SerializeField, HideInInspector]
        private uint maxHealth;

        [SerializeField, HideInInspector]
        private bool godMode;


        [ShowIf(nameof(HasTemporaryInvulnerability)), ReadOnly, BoxGroup(HealthGroup)]
        private float invulnerabilitySecondsLeft;

        [BoxGroup(HealthGroup)]
        public Color DamageColor = Color.white;

        [BoxGroup(HealthGroup)]
        public float DamageColorAmount = 1;

        [BoxGroup(DefenseGroup)]
        public float DefenseBreakStunDuration = 2;

        /// <summary>
        /// A porcentagem de vida atual em uma escala de 0 -> 1.
        /// <br/>
        /// Qualquer tentativa de mudar este valor delega a chamada para <see cref="Health"/>. 
        /// </summary>
        [ShowInInspector, BoxGroup(HealthGroup), ProgressBar(0, 1), PropertyRange(0, 1)]
        public float HealthPercentage {
            get {
                return (float) Health / MaxHealth;
            }
            set {
                Health = (uint) (value * MaxHealth);
            }
        }

        [ShowInInspector, BoxGroup(HealthGroup)]
        public bool GodMode {
            get {
                return godMode;
            }
            set {
                godMode = value;
            }
        }


        public bool IsInvulnerable => GodMode || HasTemporaryInvulnerability;

        [BoxGroup(CombatGroup)]
        public CombatantDamagedEvent OnDamaged => onDamaged;

        /// <summary>
        /// O total de vida da entidade atual.
        /// <br/>
        /// Se for setado para 0, o método <see cref="Kill"/> será chamado.
        /// <br/>
        /// Se for setado para um valor maior que <see cref="MaxHealth"/>, <see cref="Health"/> será setado 
        /// igual a <see cref="MaxHealth"/>. 
        /// </summary>
        [ShowInInspector, BoxGroup(HealthGroup)]
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
        [ShowInInspector, BoxGroup(HealthGroup)]
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

        public bool Dead => health == 0;

        public virtual uint Damage(ref DamageInfo info, IDefendable defendable = null) {
            var entity = info.Damager;
            if (defendable != null && Defending && defendable.CanDefend(this)) {
                var d = defendable.Defend(this, ref info);
                GlobalDefenseEvent.Instance.Invoke(this, info);
                if (d > CurrentStamina) {
                    CurrentStamina = 0;
                    Stun(DefenseBreakStunDuration);
                } else {
                    CurrentStamina -= d;
                }


                AnimatorUpdater.TriggerDefend();
                return 0;
            }

            if (IsInvulnerable || Ignored || Dead || info.Canceled) {
                return 0;
            }

            var attack = info.Attack;
            var multiplier = info.Multiplier * entity.DamageMultiplier.Value;

            var damage = (uint) (attack.GetDamage(this) * multiplier);
            GlobalEntityDamagedEvent.Instance.Invoke(this, entity, attack, damage);
            if (damage >= health) {
                Debug.Log($"<color=#FF0000><b>{name} killed by {entity} @ {damage}</b></color>");
                Kill();
                return health;
            }

            Debug.Log($"<color=#FF0000>{name} damaged by {entity} @ {damage}</color>");

            OnDamaged.Invoke(info);
            ColorizableRenderer.OutlineController.RegisterTimedService(DamageInvulnerabilityDuration,
                new ColorMeta(DamageColor)
            );
            Health -= damage;
            if (DamageInvulnerability) {
                SetInvulnerable(DamageInvulnerabilityDuration);
            }

            if (DamageGivesStun && damage >= DamageStunMin) {
                Stun(DamageStunDuration);
            }

            return damage;
        }

        [ShowInInspector, BoxGroup(HealthGroup)]
        public bool HasTemporaryInvulnerability => invulnerabilitySecondsLeft > 0;

        public float InvulnerabilitySecondsLeft => invulnerabilitySecondsLeft;

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

        public void Heal() {
            Heal(maxHealth - health);
        }
    }
}