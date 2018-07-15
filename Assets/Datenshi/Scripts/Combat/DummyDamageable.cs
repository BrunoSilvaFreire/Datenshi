using UnityEngine;

namespace Datenshi.Scripts.Combat {
    public class DummyDamageable : MonoBehaviour, IDamageable {
        [SerializeField]
        private uint maxHealth;

        [SerializeField]
        private uint currentHealth;

        [SerializeField]
        private bool godMode;

        [SerializeField]
        private bool ignored;

        public bool Ignored {
            get {
                return ignored;
            }
            set {
                ignored = value;
            }
        }

        public uint MaxHealth {
            get {
                return maxHealth;
            }
            set {
                maxHealth = value;
            }
        }

        public uint CurrentHealth {
            get {
                return currentHealth;
            }
            set {
                if (value < currentHealth && GodMode) {
                    return;
                }

                if (value == 0) {
                    Kill();
                } else {
                    currentHealth = value > maxHealth ? maxHealth : value;
                }
            }
        }

        public float HealthPercentage {
            get {
                return (float) currentHealth / maxHealth;
            }
            set {
                currentHealth = (uint) (maxHealth * value);
            }
        }

        public bool Dead => currentHealth == 0;

        public bool GodMode {
            get {
                return godMode;
            }
            set {
                godMode = value;
            }
        }

        public void Kill() {
            currentHealth = 0;
            Destroy(gameObject);
        }

        public void Heal(uint healthAmount) {
            CurrentHealth += healthAmount;
        }

        public uint Damage(ref DamageInfo damageInfo, IDefendable defendable = null) {
            if (GodMode) {
                return 0;
            }

            var dmg = (uint) (damageInfo.Attack.GetDamage(this) * damageInfo.Multiplier);
            CurrentHealth -= dmg;
            return dmg;
        }

        public Transform Transform => transform;

        public Vector2 Center => transform.position;

        public Vector2 GroundPosition => Center;
    }
}