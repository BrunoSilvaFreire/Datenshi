using System.Collections.Generic;
using Datenshi.Scripts.Util.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Util.Misc {
    public interface IPeriodicSpawned {
        bool Tick();
    }

    public abstract class AbstractPeriodicSpawned : MonoBehaviour, IPeriodicSpawned {
        [SerializeField]
        private float timeLeft;

        public float DefaultDuration = 5;

        public float TimeLeft {
            get {
                return timeLeft;
            }
            set {
                timeLeft = value;
            }
        }

        public void Reset() {
            timeLeft = DefaultDuration;
        }

        public bool Tick() {
            return (timeLeft -= UnityEngine.Time.deltaTime) <= 0;
        }
    }

    public abstract class PeriodicSpawner<P, T> : MonoBehaviour
        where P : ObjectPool<T> where T : Component, IPeriodicSpawned {
        public float SpawnCooldown;

        [ShowInInspector]
        public float SpawnRate {
            get {
                return 1 / SpawnCooldown;
            }
            set {
                SpawnCooldown = 1 / value;
            }
        }

        public bool Spawning;
        public P Pool;
        private float spawnCooldownLeft;

        [ShowInInspector, ReadOnly]
        private readonly List<T> beingUsed = new List<T>();

        private readonly List<T> toRemove = new List<T>();

        protected virtual float GetDeltaTime() => UnityEngine.Time.deltaTime;

        private void LateUpdate() {
            if (Spawning || permanentSpawn) {
                spawnCooldownLeft -= GetDeltaTime();
                if (spawnCooldownLeft <= 0) {
                    Spawn();
                }
            }

            toRemove.Clear();
            foreach (var sprite in beingUsed) {
                if (sprite.Tick()) {
                    toRemove.Add(sprite);
                }
            }

            foreach (var s in toRemove) {
                beingUsed.Remove(s);
                Pool.Return(s);
            }
        }

        private void Spawn() {
            spawnCooldownLeft = SpawnCooldown;
            var g = Pool.Get();
            if (g == null) {
                Debug.LogWarning("Didn't get a ghost from the pool!");
                return;
            }

            OnSpawned(g);
            beingUsed.Add(g);
        }

        protected abstract void OnSpawned(T obj);
        private bool permanentSpawn;

        public void SetPermanentSpawn(bool b) {
            permanentSpawn = b;
        }
    }
}