using UnityEngine;

namespace Datenshi.Scripts.Tutorial {
    public abstract class Tutorial : MonoBehaviour {
        public bool Playing {
            get;
        } = false;

        public void StartTutorial() {
            if (Playing) {
                return;
            }

            OnStartTutorial();
        }

        public void StopTutorial() {
            if (!Playing) {
                return;
            }

            OnStopTutorial();
        }

        protected abstract void OnStartTutorial();
        protected abstract void OnStopTutorial();
    }
}