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

    public class World : Singleton<World>, IRestartable {
        public static readonly WorldEvent WorldLoadedEvent = new WorldEvent();
        public Transform SpawnPoint;
        public AudioFX Theme;

        private void OnEnable() {
            WorldLoadedEvent.Invoke(this);
        }


        private void Start() {
            var entity = PlayerController.GetOrCreateEntity();
            entity.transform.position = SpawnPoint.position;
            AudioManager.Instance.PlayFX(Theme);
        }

        public void Restart() {
            PlayerController.GetOrCreateEntity().transform.position = GetRespawnPoint();
        }

        private Vector3 GetRespawnPoint() {
            var checkpoint = GameController.Instance.LastCheckpoint;
            if (checkpoint == null) {
                return SpawnPoint.position;
            }

            return checkpoint.Spawnpoint.position;
        }
    }
}