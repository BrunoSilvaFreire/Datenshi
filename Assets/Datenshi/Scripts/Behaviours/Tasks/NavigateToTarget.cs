using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Behaviours.Variables;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class NavigateToTarget : Action {
        public SharedCombatant Target;
        public AINavigator Navigator;
        public float DistanceThreshold;

        public override TaskStatus OnUpdate() {
            var t = Target.Value;
            if (t == null) {
                return TaskStatus.Failure;
            }

            var targetPos = t.Center;
            Navigator.Target = targetPos;
            return Vector2.Distance(targetPos, Navigator.transform.position) < DistanceThreshold ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}