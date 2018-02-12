using Entitas;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Systems {
    public class VelocityExecutionSystem : AbstractExecuteSystem<GameEntity> {
        public VelocityExecutionSystem(Contexts collector) : base(collector.game.CreateCollector(GameMatcher.AllOf(GameMatcher.Velocity, GameMatcher.View))) { }

        protected override void Execute(GameEntity entity) {
            var transform = entity.view.View.transform;
            var pos = transform.position;
            pos += (Vector3) entity.velocity.Velocity;
            transform.position = pos;
        }
    }
}