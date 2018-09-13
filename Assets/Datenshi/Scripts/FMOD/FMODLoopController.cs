using FMODUnity;
using UnityEngine;

namespace Datenshi.Scripts.FMOD {
    public class FMODLoopController : MonoBehaviour {
        public string LoopParameter = "Loop";
        public StudioEventEmitter Emitter;
        public float LoopingValue = 1, NotLoopingValue = 0;

        [SerializeField]
        private bool looping;

        public bool Looping {
            get {
                return looping;
            }
            set {
                looping = value;
                UpdateFMOD();
            }
        }

        public void EnableLoop() {
            Looping = true;
        }

        public void DisableLoop() {
            Looping = false;
        }

        private void Start() {
            UpdateFMOD();
        }

        private void UpdateFMOD() {
            var instance = Emitter.EventInstance;
            if (!instance.isValid()) {
                Debug.LogWarning("Instance is invalid! preventing");
                return;
            }

            var val = looping ? LoopingValue : NotLoopingValue;
            instance.setParameterValue(LoopParameter, val).PrintIfError();
        }
    }
}