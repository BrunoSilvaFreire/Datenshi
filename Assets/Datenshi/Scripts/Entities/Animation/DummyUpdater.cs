using Datenshi.Scripts.Combat;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Animation {
    public class DummyUpdater : CombatantAnimatorUpdater {
        protected override void UpdateAnimator(Animator anim) { }

        public override void TriggerAttack() { }

        public override void TriggerAttack(string attack) { }

        public override void SetDefend(bool defend) { }

        public override void SetTrigger(string key) { }

        public override void TriggerDeflect() { }

        public override void TriggerCounter() { }

        public override void TriggerDeath() {
            Destroy(transform.parent.gameObject);
        }

        public override void TriggerSpawn() { }
    }
}