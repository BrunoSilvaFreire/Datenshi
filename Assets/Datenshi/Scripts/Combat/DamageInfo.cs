using System;
using Datenshi.Scripts.Combat.Attacks;
using UnityEngine;

namespace Datenshi.Scripts.Combat {
    [Serializable]
    public class DamageInfo {
        public DamageInfo(IDamageSource attack, float multiplier, IDamageable damaged, IDamageDealer damager) {
            this.attack = attack;
            this.multiplier = multiplier;
            this.canceled = false;
            this.damaged = new SerializableDamageable(damaged);
            this.damager = new SerializableDamageDealer(damager);
        }

        [SerializeField]
        private IDamageSource attack;

        [SerializeField]
        private float multiplier;

        [SerializeField]
        private SerializableDamageable damaged;

        [SerializeField]
        private SerializableDamageDealer damager;

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

        public IDamageDealer Damager => damager.Value;

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