using System.Collections.Generic;
using Datenshi.Scripts.Entities.Components.Movement;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.StateMachine;
using Entitas;

namespace Datenshi.Scripts.Entities.Systems.Initialize {
    public class InitializeGroundMovementSystem : ReactiveSystem<GameEntity> {
        public InitializeGroundMovementSystem(Contexts contexts) : base(contexts.game) { }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) {
            return context.CreateCollector(GameMatcher.GroundMovement.Added());
        }

        protected override bool Filter(GameEntity entity) {
            return true;
        }

        protected override void Execute(List<GameEntity> entities) {
            foreach (var gameEntity in entities) {
                var m = gameEntity.groundMovement;
                m.StateMachine = new StateMachine<GroundState, GameEntity>(new NormalGroundState(), gameEntity);
                m.Controller = m.ControllerPrefab.Clone(gameEntity.view.View.transform);
            }
        }
    }
}