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

        [SerializeField, HideInInspector]
        private byte totalSprites;

        public GhostingSprite GhostPrefab;
        private List<GhostingSprite> spritesPool = new List<GhostingSprite>();
        private float lastSpawn;

        [ShowInInspector]
        public byte TotalSprites {
            get {
                return totalSprites;
            }
            set {
                totalSprites = value;
            }
        }

        private void Update() {
            var now = Time.time;
            var delay = now - lastSpawn;
            if (delay >= SpawnRate) {
                Spawn();
            }
        }

        private void Spawn() {
            lastSpawn = Time.time;
            var g = GetGhost();
            if (g == null) {
                
            }
        }

        private GhostingSprite GetGhost() {
            throw new System.NotImplementedException();
        }
    }
}