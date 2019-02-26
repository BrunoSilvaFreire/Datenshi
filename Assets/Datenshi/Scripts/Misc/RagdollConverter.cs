using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class RagdollConverter : MonoBehaviour {
        public Object[] ToDestroy;
        public Behaviour[] ToEnable;
        public Rigidbody Rigidbody;
        public bool Launch;
        public Vector3 LaunchDirection, LaunchDirectionRandomization;
        public Vector3 RotationDirection, RotationDirectionRandomization;

        public void Convert() {
            foreach (var o in ToDestroy) {
                Destroy(o);
            }

            if (Rigidbody == null) {
                Rigidbody = this.GetOrAddComponent<Rigidbody>();
            }

            Rigidbody.isKinematic = false;
            if (Launch) {
                Rigidbody.velocity = LaunchDirection + RandomVector(LaunchDirectionRandomization);
                Rigidbody.angularVelocity = RotationDirection + RandomVector((RotationDirectionRandomization));
            }

            foreach (var component in ToEnable) {
                component.enabled = true;
            }
        }

        private Vector3 RandomVector(Vector3 dir) {
            return new Vector3(
                Random.value * dir.x,
                Random.value * dir.y,
                Random.value * dir.z);
        }
    }
}