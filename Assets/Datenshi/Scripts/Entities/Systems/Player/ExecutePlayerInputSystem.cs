using Entitas;

namespace Datenshi.Scripts.Entities.Systems.Player {
    public class ExecutePlayerInputSystem : AbstractExecuteSystem<GameEntity> {
        public ExecutePlayerInputSystem(Contexts contexts) : base(contexts.game.CreateCollector(GameMatcher.Player)) { }

        protected override void Execute(GameEntity entity) {
            entity.player.StateMachine.Execute();
        }
    }
}