using Shiroi.FX.Features;
using UnityEngine;

namespace Datenshi.Scripts.Entities.FX {
    public class EntityFeature : EffectFeature {
        public EntityFeature(Entity entity, params PropertyName[] tags) : base(tags) {
            Entity = entity;
        }

        public Entity Entity {
            get;
        }
    }
}