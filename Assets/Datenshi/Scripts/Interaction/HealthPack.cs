using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Interaction {
    public sealed class HealthPack : Collectable {
        public uint HealthAmount = 6;
        public ParticleSystem ToInstantiate;
        protected override void Collect(MovableEntity movableEntity) {
            movableEntity.Heal(HealthAmount);
            ToInstantiate.Clone(transform.position);
            var particleFollow = ToInstantiate.GetComponentInChildren<ParticleFollowTransform>();
            if (particleFollow == null) {
                return;
            }
            particleFollow.Entity = movableEntity;
        }
    }
}