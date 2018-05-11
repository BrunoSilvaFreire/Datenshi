using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class ParticleFollowTransform : MonoBehaviour {
        public LivingEntity Entity;
        public ParticleSystem System;
        public bool Execute;
        public float Acceleration;
        public float MaxSpeed;
        public float DestroyDistance = .1F;
        public uint MaxParticles = 15;
        private ParticleSystem.Particle[] cache;

        private void Start() {
            cache = new ParticleSystem.Particle[MaxParticles];
        }

        private void Update() {
            if (!Execute || Entity == null || System == null) {
                return;
            }

            var targetPos = (Vector3) Entity.Center;
            var size = System.GetParticles(cache);
            var finalSize = size;
            for (var i = 0; i < MaxParticles; i++) {
                var p = cache[i];
                var vel = p.velocity;
                var dist = p.position - targetPos;
                if (dist.magnitude <= DestroyDistance) {
                    finalSize--;
                    continue;
                }

                vel += dist * Acceleration * Time.deltaTime;
                p.velocity = Vector3.ClampMagnitude(vel, MaxSpeed);
                cache[i] = p;
            }

            System.SetParticles(cache, finalSize);
        }
    }
}