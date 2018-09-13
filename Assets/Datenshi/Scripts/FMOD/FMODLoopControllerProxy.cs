using UnityEngine;
using FMODUnity;

namespace Datenshi.Scripts.FMOD {
    public class FMODLoopControllerProxy : MonoBehaviour {
        public FMODLoopController Target;

        public void EnableLoop() {
            if (Target == null) {
                return;
            }

            Target.EnableLoop();
        }

        public void DisableLoop() {
            if (Target == null) {
                return;
            }

            Target.DisableLoop();
        }

        public void SetLoopParameter(string value) {
            if (Target == null) {
                return;
            }

            Target.LoopParameter = value;
        }

        public void SetEmitter(StudioEventEmitter value) {
            if (Target == null) {
                return;
            }

            Target.Emitter = value;
        }

        public void SetLoopingValue(float value) {
            if (Target == null) {
                return;
            }

            Target.LoopingValue = value;
        }

        public void SetNotLoopingValue(float value) {
            if (Target == null) {
                return;
            }

            Target.NotLoopingValue = value;
        }

        public void SetLooping(bool value) {
            if (Target == null) {
                return;
            }

            Target.Looping = value;
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