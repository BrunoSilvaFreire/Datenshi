using BehaviorDesigner.Runtime.Tasks;
using Lunari.Tsuki;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class CloseToCamera : Conditional {
        public float MinRequiredDistance = 20;
        public float MaxAllowedDistance = 50;
        public Transform ToCheck;
        private bool active;

        public override TaskStatus OnUpdate() {
            var m = Camera.main;
            return active ? HandleAsActive(m) : HandleAsInactive(m);
        }

        private TaskStatus HandleAsActive(Camera camera) {
            var far = Vector2.Distance(transform.position, camera.transform.position) >= MaxAllowedDistance;
            if (far) {
                active = false;
            }
            return far ? TaskStatus.Failure : TaskStatus.Success;
        }

        private TaskStatus HandleAsInactive(Camera camera) {
            var close = Vector2.Distance(transform.position, camera.transform.position) <= MinRequiredDistance;
            if (close) {
                active = true;
            }
            return close ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnDrawGizmos() {
            var m = Camera.main;
            if (m == null) {
                return;
            }

            var a = ToCheck.position;

            var b = m.transform.position;
            var d = Vector2.Distance(a, b);
            var r = active ? MaxAllowedDistance : MinRequiredDistance;
            Debugging.DrawWireCircle2D(a, r, Color.yellow);
            var valid = active ? r <= d : d >= r;
            var c = valid ? Color.red : Color.green;
            Debug.DrawLine(a, b, c);
        }
    }
}