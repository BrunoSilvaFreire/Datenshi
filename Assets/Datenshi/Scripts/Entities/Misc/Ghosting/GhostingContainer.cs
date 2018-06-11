﻿using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Misc.Ghosting {
    /// <summary>
    /// container for ghosting sprites. triggers a new ghosting object over a set amount of time, referencing a sprite renderer's current sprite
    /// combined, this forms a trailing effect behind the player 
    /// </summary>
    public class GhostingContainer : MonoBehaviour {
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
        public GhostPool Pool;
        private float spawnCooldownLeft;
        public MovableEntity Entity;

        [ShowInInspector, ReadOnly]
        private readonly List<GhostingSprite> usedSprites = new List<GhostingSprite>();

        private readonly List<GhostingSprite> toRemove = new List<GhostingSprite>();


        private void LateUpdate() {
            if (Spawning || permanentSpawn) {
                spawnCooldownLeft -= Entity.DeltaTime;
                if (spawnCooldownLeft <= 0) {
                    Spawn();
                }
            }

            toRemove.Clear();
            foreach (var sprite in usedSprites) {
                sprite.TimeLeft -= Entity.DeltaTime;
                if (sprite.TimeLeft <= 0) {
                    toRemove.Add(sprite);
                }
            }

            foreach (var s in toRemove) {
                usedSprites.Remove(s);
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

            g.Setup(Entity);
            usedSprites.Add(g);
        }

        private bool permanentSpawn;

        public void SetPermanentSpawn(bool b) {
            permanentSpawn = b;
        }
    }
}