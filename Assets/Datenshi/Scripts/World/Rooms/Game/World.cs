using System;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util.Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.World.Rooms.Game {
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
            var entity = PlayerController.GetOrCreateEntity();
            Debug.Log("Entity = " + entity);
            entity.transform.position = SpawnPoint.position;
            AudioManager.Instance.PlayFX(Theme);
        }
    }
}