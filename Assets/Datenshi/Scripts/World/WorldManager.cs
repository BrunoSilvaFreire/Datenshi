using System;
using Datenshi.Scripts.Util.Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.World {
    [Serializable]
    public class AreaChangedEvent : UnityEvent<Area, Area> {
        public static readonly AreaChangedEvent Instance = new AreaChangedEvent();
        private AreaChangedEvent() { }
    }

    public class WorldManager : Singleton<WorldManager> {
        [SerializeField, HideInInspector]
        private Area currentArea;

        public Area CurrentArea {
            get {
                return currentArea;
            }
            set {
                currentArea = value;
            }
        }
    }
}