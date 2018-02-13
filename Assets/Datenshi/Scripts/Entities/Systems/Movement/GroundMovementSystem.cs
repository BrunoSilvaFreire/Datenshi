using Entitas;

namespace Datenshi.Scripts.Entities.Systems.Movement {
    public class GroundMovementSystem : AbstractExecuteSystem<GameEntity> {
        public GroundMovementSystem(Contexts contexts) : base(contexts.game.CreateCollector(GameMatcher.GroundMovement)) { }
        protected override void Execute(GameEntity entity) {
            entity.groundMovement.StateMachine.Execute();
        }
    }
}