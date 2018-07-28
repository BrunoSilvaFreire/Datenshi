using System;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Game.Restart;
using Datenshi.Scripts.Util.Singleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.World.Rooms.Game {
    [Serializable]
    public class WorldEvent : UnityEvent<World> { }

    public class World : Singleton<World> {
        public static readonly WorldEvent WorldLoadedEvent = new WorldEvent();
        public Transform SpawnPoint;
        public AudioFX Theme;

        public Light SunLight;

        private void OnEnable() {
            WorldLoadedEvent.Invoke(this);
        }


        private void Start() {
            var entity = PlayerController.GetOrCreateEntity();
            entity.transform.position = SpawnPoint.position;
            AudioManager.Instance.PlayFX(Theme);
        }

    }
}