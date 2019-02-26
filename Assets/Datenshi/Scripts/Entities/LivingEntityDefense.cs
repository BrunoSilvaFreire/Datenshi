using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity {
        public const string DefenseGroup = "Defense";
        private bool defending;

        [BoxGroup(CombatGroup)]
        public float MinDefenseRequired = 0.1F;

        [ShowInInspector, ReadOnly, BoxGroup(CombatGroup)]
        public bool CanDefend => CurrentStamina > MinDefenseRequired;

        public float DefendingFor {
            get {
                if (!defending) {
                    return 0;
                }

                return Time.time - LastDefenseStart;
            }
        }


        [BoxGroup(DefenseGroup)]
        public float DefenseRecoveryMultiplier = 2;

        public float LastDefenseStart {
            get;
            private set;
        }

        [ShowInInspector, ReadOnly, BoxGroup(CombatGroup)]
        public bool Defending {
            get {
                return defending;
            }
            set {
                if (defending == value) {
                    return;
                }

                defending = value;
                if (defending) {
                    LastDefenseStart = Time.time;
                }
            }
        }

        private void InitDefense() {
            defenseHandle = GetStaminaHandle(DefenseStaminaConsumption);
        }


        public void BreakDefense() { }
        private StaminaHandle defenseHandle;

        [BoxGroup(DefenseGroup)]
        public float DefenseStaminaConsumption;

        private void UpdateDefense() {
            defenseHandle.Active = defending;
            if (updater == null) {
                return;
            }

            updater.SetDefending(defending);
        }
    }
}