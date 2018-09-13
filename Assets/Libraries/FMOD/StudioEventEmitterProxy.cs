using UnityEngine;
using System;

namespace FMODUnity {
    public class StudioEventEmitterProxy : MonoBehaviour {
        public StudioEventEmitter Target;

        public void Play() {
            Target.Play();
        }

        public void Stop() {
            Target.Stop();
        }

        public void SetParameter(string name, float value) {
            Target.SetParameter(name, value);
        }

        public void SetEvent(string value) {
            if (Target == null) {
                return;
            }

            Target.Event = value;
        }

        public void SetPlayEvent(EmitterGameEvent value) {
            if (Target == null) {
                return;
            }

            Target.PlayEvent = value;
        }

        public void SetStopEvent(EmitterGameEvent value) {
            if (Target == null) {
                return;
            }

            Target.StopEvent = value;
        }

        public void SetCollisionTag(string value) {
            if (Target == null) {
                return;
            }

            Target.CollisionTag = value;
        }

        public void SetAllowFadeout(bool value) {
            if (Target == null) {
                return;
            }

            Target.AllowFadeout = value;
        }

        public void SetTriggerOnce(bool value) {
            if (Target == null) {
                return;
            }

            Target.TriggerOnce = value;
        }

        public void SetPreload(bool value) {
            if (Target == null) {
                return;
            }

            Target.Preload = value;
        }

        public void SetParams(ParamRef[] value) {
            if (Target == null) {
                return;
            }

            Target.Params = value;
        }

        public void SetOverrideAttenuation(bool value) {
            if (Target == null) {
                return;
            }

            Target.OverrideAttenuation = value;
        }

        public void SetOverrideMinDistance(float value) {
            if (Target == null) {
                return;
            }

            Target.OverrideMinDistance = value;
        }

        public void SetOverrideMaxDistance(float value) {
            if (Target == null) {
                return;
            }

            Target.OverrideMaxDistance = value;
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