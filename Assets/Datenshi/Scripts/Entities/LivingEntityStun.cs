using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity {
        [BoxGroup(CombatGroup)]
        public bool DamageGivesStun;

        [SerializeField]
        [BoxGroup(CombatGroup)]
        public bool Stunned {
            get;
            private set;
        }

        [ShowIf("DamageGivesStun"), BoxGroup(CombatGroup)]
        public uint DamageStunMin = 10;

        [ShowIf("DamageGivesStun"), BoxGroup(CombatGroup)]
        public float DamageStunDuration = 1;

        [ShowIf("DamageGivesStun"), BoxGroup(CombatGroup), ReadOnly]
        private float totalStunTimeLeft;

        public virtual void Stun(float duration) {
            if (GodMode) {
                return;
            }

            Stunned = true;
            totalStunTimeLeft += duration;
        }

        private void UpdateStun() {
            if (Stunned) {
                totalStunTimeLeft -= Time.deltaTime;
                if (totalStunTimeLeft < 0) {
                    Stunned = false;
                }
            }
        }
    }
}