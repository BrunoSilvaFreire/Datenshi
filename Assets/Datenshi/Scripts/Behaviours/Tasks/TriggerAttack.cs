using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class TriggerAttack : Action {
        public string AttackTrigger;
        public LivingEntity Entity;
        public float Delay = 3;
        private float lastUsed;

        public override TaskStatus OnUpdate() {
            var now = Time.time;
            if (now - lastUsed < Delay) {
                return TaskStatus.Running;
            }

            lastUsed = now;
            Entity.AnimatorUpdater.TriggerAttack(AttackTrigger);
            return TaskStatus.Success;
        }
    }
}