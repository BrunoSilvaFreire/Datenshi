using System;
using Datenshi.Scripts.Combat.Attacks;
using UnityEngine;

namespace Datenshi.Scripts.Combat {
    [Serializable]
    public struct DamageInfo {
        public DamageInfo(Attack attack, float multiplier, ICombatant damaged, ICombatant damager) {
            this.attack = attack;
            this.multiplier = multiplier;
            this.canceled = false;
            this.damaged = new SerializableCombatant(damaged);
            this.damager = new SerializableCombatant(damager);
        }

        [SerializeField]
        private Attack attack;

        [SerializeField]
        private float multiplier;

        [SerializeField]
        private SerializableCombatant damaged;

        [SerializeField]
        private SerializableCombatant damager;

        [SerializeField]
        private bool canceled;

        public Attack Attack => attack;

        public float Multiplier {
            get {
                return multiplier;
            }
            set {
                multiplier = value;
            }
        }

        public ICombatant Damaged => damaged.Value;

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