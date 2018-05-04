using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class StayCloseToTarget : Action {
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
            return Vector2.Distance(targetPos, Navigator.transform.position) < DistanceThreshold
                ? TaskStatus.Success
                : TaskStatus.Running;
        }

        public override void OnDrawGizmos() {
            var t = Target.Value;
            if (t == null) {
                return;
            }

            var targetPos = t.Center;
            Navigator.Target = targetPos;
            Debug.DrawLine(targetPos, Navigator.transform.position);
            var dist = Vector2.Distance(targetPos, Navigator.transform.position);
            var msg = string.Format("Distance {0}-{1}: {2}/{3}", Navigator.name, t.AnimatorUpdater.gameObject.name,
                dist,
                DistanceThreshold);
            DebugUtil.DrawLabel(targetPos, msg);
        }
    }
}