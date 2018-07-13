using System;
using Datenshi.Scripts.Combat.Attacks;
using UnityEngine;

namespace Datenshi.Scripts.Combat {
    [Serializable]
    public struct DamageInfo {
        public DamageInfo(IDamageSource attack, float multiplier, IDamageable damaged, ICombatant damager) {
            this.attack = attack;
            this.multiplier = multiplier;
            this.canceled = false;
            this.damaged = new SerializableDamageable(damaged);
            this.damager = new SerializableCombatant(damager);
        }

        [SerializeField]
        private IDamageSource attack;

        [SerializeField]
        private float multiplier;

        [SerializeField]
        private SerializableDamageable damaged;

        [SerializeField]
        private SerializableCombatant damager;

        [SerializeField]
        private bool canceled;

        public IDamageSource Attack => attack;

        public float Multiplier {
            get {
                return multiplier;
            }
            set {
                multiplier = value;
            }
        }

        public IDamageable Damaged => damaged.Value;

        public ICombatant Damager => damager.Value;

        public bool Canceled {
            get {
                return canceled;
            }
            set {
                canceled = value;
            }
        }
    }
}