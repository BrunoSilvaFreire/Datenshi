using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.FX;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class Follow : Action {
        public AINavigator Navigator;
        public SharedLivingEntity Target;
        public MovableEntity Entity;
        public Transform Override;
        public float TeleportThreshold = 8;
        public float IdleThreshold = .5F;
        public EntityEffect TeleportEffect;

        public override TaskStatus OnUpdate() {
            var t = Target.Value;
            if (t == null) {
                return TaskStatus.Failure;
            }

            var p = Entity.InputProvider as DummyInputProvider;
            if (p == null) {
                return TaskStatus.Failure;
            }

            var targetPos = Navigator.GetFavourablePosition(Override != null ? (Vector2) Override.position : t.Center);
            var d = Vector2.Distance(Entity.Center, targetPos);
            if (d > TeleportThreshold) {
                Entity.transform.position = targetPos;
                if (TeleportEffect != null) {
                    TeleportEffect.Execute(Entity);
                }
                return TaskStatus.Running;
            }

            if (d > IdleThreshold) {
                Navigator.SetTarget(targetPos);
                Navigator.Execute(Entity, p);
            } else {
                p.Reset();
            }

            return TaskStatus.Running;
        }
    }
}