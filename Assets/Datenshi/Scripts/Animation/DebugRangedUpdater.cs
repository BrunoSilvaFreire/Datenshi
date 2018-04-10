using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Animation {
    public class DebugRangedUpdater : EntityAnimatorUpdater {
        public Attack Attack;
        public MovableEntity Entity;
        protected override void UpdateAnimator(Animator anim) { }

        public override void TriggerAttack() {
            Attack.Execute(Entity);
        }

        public override void SetDefend(bool defend) { }
        public override void TriggerDeflect() { }

        public override void TriggerCounter() { }
    }
}