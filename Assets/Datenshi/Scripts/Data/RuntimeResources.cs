using System;
using Datenshi.Scripts.Util.Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Data {
    [Serializable]
    public class GamePausedChangeEvent : UnityEvent<bool> {
        public static readonly GamePausedChangeEvent Instance = new GamePausedChangeEvent();
        private GamePausedChangeEvent() { }
    }

    public class RuntimeResources : Singleton<RuntimeResources> {
        public bool AllowPlayerInput;
        public bool AllowAIInput;

        [SerializeField]
        private bool paused;

        public bool Paused {
            get {
                return paused;
            }
            set {
                if (paused == value) {
                    return;
                }

                paused = value;
                GamePausedChangeEvent.Instance.Invoke(value);
            }
        }

        public void TogglePaused() {
            Paused = !Paused;
        }
    }
}