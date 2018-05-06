using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class CloseToCamera : Conditional {
        public float MinRequiredDistance = 20;
        public Transform ToCheck;
        public override TaskStatus OnUpdate() {
            var m = Camera.main;
            var close = Vector2.Distance(transform.position, m.transform.position) <= MinRequiredDistance;
            return close ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnDrawGizmos() {
            var m = Camera.main;
            var a = ToCheck.position;
            var b = m.transform.position;
            var d = Vector2.Distance(a, b);
            DebugUtil.DrawWireCircle2D(a, MinRequiredDistance, Color.yellow);
            var c = d > MinRequiredDistance ? Color.red : Color.green;
            Debug.DrawLine(a, b, c);
        }
    }
}