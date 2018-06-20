using System;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Util.Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.World {
    [Serializable]
    public class WorldEvent : UnityEvent<World> { }

    public class World : Singleton<World> {
        public static readonly WorldEvent WorldLoadedEvent = new WorldEvent();
        public Transform SpawnPoint;
        public AudioFX Theme;
        private void OnEnable() {
            WorldLoadedEvent.Invoke(this);
        }

        private void Start() {
            AudioManager.Instance.PlayFX(Theme);
        }
    }
}