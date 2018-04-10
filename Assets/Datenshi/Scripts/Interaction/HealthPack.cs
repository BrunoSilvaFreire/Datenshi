using Datenshi.Scripts.Entities;

namespace Datenshi.Scripts.Interaction {
    public sealed class HealthPack : Collectable {
        public uint HealthAmount = 6;

        protected override void Collect(MovableEntity movableEntity) {
            movableEntity.Heal(HealthAmount);
        }
    }
}