using UnityEngine;

namespace Datenshi.Scripts.FMOD {
    public class FMODRestarterProxy : MonoBehaviour {
        public FMODRestarter Target;

        public void Restart() {
            Target.Restart();
        }

        public void SetValues(FMODParameterValue[] value) {
            if (Target == null) {
                return;
            }

            Target.Values = value;
        }

        public void SetUseGUILayout(bool value) {
            if (Target == null) {
                return;
            }

            Target.useGUILayout = value;
        }

        public void SetRunInEditMode(bool value) {
            if (Target == null) {
                return;
            }

            Target.runInEditMode = value;
        }

        public void SetEnabled(bool value) {
            if (Target == null) {
                return;
            }

            Target.enabled = value;
        }

        public void SetTag(string value) {
            if (Target == null) {
                return;
            }

            Target.tag = value;
        }

        public void SetName(string value) {
            if (Target == null) {
                return;
            }

            Target.name = value;
        }

        public void SetHideFlags(HideFlags value) {
            if (Target == null) {
                return;
            }

            Target.hideFlags = value;
        }
    }
}