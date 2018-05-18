using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Misc.Ghosting {
    /// <summary>
    /// container for ghosting sprites. triggers a new ghosting object over a set amount of time, referencing a sprite renderer's current sprite
    /// combined, this forms a trailing effect behind the player 
    /// </summary>
    public class GhostingContainer : MonoBehaviour {
        public float SpawnRate;
        public bool Spawning;
        public GhostPool Pool;
        private float lastSpawn;
        public MovableEntity Entity;

        [ShowInInspector, ReadOnly]
        private readonly List<GhostingSprite> usedSprites = new List<GhostingSprite>();

        private readonly List<GhostingSprite> toRemove = new List<GhostingSprite>();


        private void Update() {
            if (Spawning) {
                var now = Time.time;
                var delay = now - lastSpawn;
                if (delay >= SpawnRate) {
                    Spawn();
                }
            }

            toRemove.Clear();
            foreach (var sprite in usedSprites) {
                sprite.TimeLeft -= Time.deltaTime;
                if (sprite.TimeLeft < 0) {
                    toRemove.Add(sprite);
                }
            }

            foreach (var s in toRemove) {
                usedSprites.Remove(s);
                Pool.Return(s);
            }
        }

        private void Spawn() {
            lastSpawn = Time.time;
            var g = Pool.Get();
            if (g == null) {
                Debug.LogWarning("Didn't get a ghost from the pool!");
                return;
            }

            g.Setup(Entity);
            usedSprites.Add(g);
        }
    }
}