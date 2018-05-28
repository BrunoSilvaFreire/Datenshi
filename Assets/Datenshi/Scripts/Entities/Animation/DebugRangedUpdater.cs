using System.Linq;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Animation {
    public class DebugRangedUpdater : CombatantAnimatorUpdater {
        public Attack[] Attacks;
        public LivingEntity Entity;
        protected override void UpdateAnimator(Animator anim) { }

        public override void TriggerAttack() {
            Attacks[0].Execute(Entity);
        }

        public override void TriggerAttack(string attack) {
            var found = Attacks.FirstOrDefault(a => a.name == attack);
            if (found != null) {
                found.Execute(Entity);
            }
        }

        public override void SetDefend(bool defend) { }
        public override void SetTrigger(string key) { }

        public override void TriggerDeath() {
            Destroy(Entity.gameObject);
        }

        public override void TriggerSpawn() { }
        public override void SetBool(string key, bool value) {
        }
    }
}