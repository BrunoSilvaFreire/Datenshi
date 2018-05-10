using System;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Datenshi.Scripts.World.Rooms.Misc {
    public class Spawner : AbstractRoomMember {
        public enum SpawnMode {
            Random,
            PredefinedLocation
        }

        public SpawnMode Mode;
        public Wave Wave;

        private void Start() {
            Room.OnObjectEnter.AddListener(OnEnter);
        }

        private void OnEnter(Collider2D arg0) {
            var e = arg0.GetComponentInChildren<Entity>();
            if () { }
        }

        [ShowIf(nameof(IsPredefinedLocation))]
        public Vector2[] Locations;

        public bool IsRandom => Mode == SpawnMode.Random;
        public bool IsPredefinedLocation => Mode == SpawnMode.PredefinedLocation;
        private byte current;
        private float lastSpawnTime;

        private void Update() {
            if (CurrentGroup.IsTimeBased) {
                var now = Time.time;
                if (now - lastSpawnTime > CurrentGroup.SecondsDelay) {
                    Spawn();
                    return;
                }
            }

            if (CurrentGroup.IsWaitForPrevious && current > 0) { }
        }

        public void Spawn() {
            CurrentGroup.Spawn(this);
            current++;
            lastSpawnTime = Time.time;
            if (current < Wave.TotalGroups - 1 && NextGroup.IsSimultaneous) {
                Spawn();
            }
        }

        public WaveGroup CurrentGroup => Wave.Groups[current];
        public WaveGroup LastGroup => Wave.Groups[current - 1];
        public WaveGroup NextGroup => Wave.Groups[current + 1];

        public Vector2 GetSpawnLocation() {
            switch (Mode) {
                case SpawnMode.Random:
                    return GetRandomSpawnLocation();
                case SpawnMode.PredefinedLocation:
                    return GetRandomPredefinedLocation();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDrawGizmos() {
            if (!IsRandom) {
                return;
            }

            foreach (var location in Locations) {
                GizmosUtil.DrawBox2DWire(location, Vector2.one, Color.red);
            }
        }

        private Vector2 GetRandomPredefinedLocation() {
            return Locations.RandomElement();
        }

        private Vector2 GetRandomSpawnLocation() {
            var w = Room.Width;
            var h = Room.Height;
            var xIncrement = (Random.value * w) - w / 2;
            var yIncrement = Random.value * h - h / 2;
            var pos = Room.Area.offset;
            pos.x += xIncrement;
            pos.y += yIncrement;
            return pos;
        }
    }

    [Serializable]
    public class Wave {
        public WaveGroup[] Groups;

        public int TotalGroups => Groups.Length;
    }

    [Serializable]
    public class WaveGroup {
        public enum WaveSpawnMode {
            WaitForPrevious,
            TimeBased,
            Simultaneous
        }

        public WaveSpawnMode Mode;

        public GameObject Prefab;

        public bool IsTimeBased => Mode == WaveSpawnMode.TimeBased;
        public bool IsWaitForPrevious => Mode == WaveSpawnMode.WaitForPrevious;
        public bool IsSimultaneous => Mode == WaveSpawnMode.Simultaneous;

        public byte Count = 3;

        [ShowInInspector, ReadOnly]
        public GameObject[] Spawned {
            get;
            private set;
        }

        [ShowIf(nameof(IsTimeBased))]
        public float SecondsDelay;

        public void Spawn(Spawner spawner) {
            Spawned = new GameObject[Count];
            for (byte i = 0; i < Count; i++) {
                var loc = spawner.GetSpawnLocation();
                var obj = Prefab.Clone(loc);
                var c = obj.GetComponentInChildren<ICombatant>();
                c?.AnimatorUpdater.TriggerSpawn();
                Spawned[i] = obj;
            }
        }
    }
}