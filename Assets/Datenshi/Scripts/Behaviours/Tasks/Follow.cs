using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class Follow : Action {
        public AINavigator Navigator;
        public SharedLivingEntity Target;
        public MovableEntity Entity;
        public float TeleportThreshold = 8;

        public override TaskStatus OnUpdate() {
            var t = Target.Value;
            if (t == null) {
                return TaskStatus.Failure;
            }

            var targetPos = Navigator.GetFavourablePosition(t);
            if (Vector2.Distance(Entity.Center, targetPos) > TeleportThreshold) {
                Entity.transform.position = targetPos;
                return TaskStatus.Running;
            }

            Navigator.SetTarget(targetPos);
            Navigator.Execute(Entity, (DummyInputProvider) Entity.InputProvider);

            return TaskStatus.Running;
        }
    }
}