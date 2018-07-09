using System.Linq;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Animation {
    public class DummyUpdater : CombatantAnimatorUpdater {
        public Attack[] Attacks;
        public LivingEntity Entity;

        public override void TriggerAttack(string attack = DefaultAttackName) {
            var found = Attacks.FirstOrDefault(a => a.name == attack);
            if (found != null) {
                found.Execute(Entity);
            }
        }

        protected override void UpdateAnimator(Animator anim) { }
        
        public override void SetDefending(bool defend) { }

        public override void SetTrigger(string key) { }

        public override void TriggerDeath() {
            Destroy(transform.parent.gameObject);
        }

        public override void TriggerSpawn() { }
        public override void SetBool(string key, bool value) { }

        public override void TriggerDefend() { }
    }
}