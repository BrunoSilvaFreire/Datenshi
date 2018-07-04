using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Tutorial {
    public abstract class Tutorial : MonoBehaviour {
        [ShowInInspector]
        public bool Playing {
            get;
            private set;
        } = false;

        public void StartTutorial() {
            if (Playing) {
                Debug.LogWarning($"Attempted to start tutorial {name} but it\'s already playing.");
                return;
            }

            Playing = true;
            OnStartTutorial();
        }

        public void StopTutorial() {
            if (!Playing) {
                Debug.LogWarning($"Attempted to stop tutorial {name} but it\'s now playing.");
                return;
            }
            Playing = false;
            OnStopTutorial();
        }

        protected abstract void OnStartTutorial();
        protected abstract void OnStopTutorial();
    }
}