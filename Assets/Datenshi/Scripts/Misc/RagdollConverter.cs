using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class RagdollConverter : MonoBehaviour {
        public Object[] ToDestroy;
        public Behaviour[] ToEnable;
        public Rigidbody Rigidbody;
        public bool Launch;
        public Vector3 LaunchDirection;
        public Vector3 RotationDirection;

        public void Convert() {
            foreach (var o in ToDestroy) {
                Destroy(o);
            }

            if (Rigidbody == null) {
                Rigidbody = this.GetOrAddComponent<Rigidbody>();
            }

            Rigidbody.isKinematic = false;
            if (Launch) {
                Rigidbody.velocity = LaunchDirection;
                Rigidbody.angularVelocity = RotationDirection;
            }
            foreach (var component in ToEnable) {
                component.enabled = true;
            }
        }
    }
}