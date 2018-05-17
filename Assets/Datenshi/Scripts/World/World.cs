using System;
using Datenshi.Scripts.Util.Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.World {
    [Serializable]
    public class WorldEvent : UnityEvent<World> { }

    public class World : Singleton<World> {
        public static readonly WorldEvent WorldLoadedEvent = new WorldEvent();
        public Transform SpawnPoint;

        private void OnEnable() {
            WorldLoadedEvent.Invoke(this);
        }
    }
}