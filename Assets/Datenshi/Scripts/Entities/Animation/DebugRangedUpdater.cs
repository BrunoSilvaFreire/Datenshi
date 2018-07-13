using System.Linq;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Animation {
    public class DebugRangedUpdater : CombatantAnimatorUpdater {
        public ActiveSkill[] Skills;
        public LivingEntity Entity;
        protected override void UpdateAnimator(Animator anim) { }

        public override void TriggerAttack(string attack = DefaultAttackName) {
            var found = Skills.FirstOrDefault(a => a.name == attack);
            if (found != null) {
                found.Execute(Entity);
            }
        }

        public override void SetDefending(bool defend) { }
        public override void SetTrigger(string key) { }

        public override void TriggerDeath() {
            Destroy(Entity.gameObject);
        }

        public override void TriggerSpawn() { }
        public override void SetBool(string key, bool value) { }
        public override void TriggerDefend() {
            
        }
    }
}