using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity {
        [TitleGroup(CombatGroup)]
        public bool DamageGivesStun;

        [SerializeField]
        [TitleGroup(CombatGroup)]
        public bool Stunned {
            get;
            private set;
        }

        [ShowIf("DamageGivesStun"), TitleGroup(CombatGroup)]
        public uint DamageStunMin = 10;

        [ShowIf("DamageGivesStun"), TitleGroup(CombatGroup)]
        public float DamageStunDuration = 1;

        [ShowIf("DamageGivesStun"), TitleGroup(CombatGroup), ReadOnly]
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