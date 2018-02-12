using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Systems.Initialize {
    public class InitializeViewSystem : ReactiveSystem<GameEntity> {
        private GameContext context;

        public InitializeViewSystem(Contexts contexts) : base(contexts.game) {
            this.context = contexts.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) {
            return context.CreateCollector(GameMatcher.View.Added());
        }

        protected override bool Filter(GameEntity entity) {
            return true;
        }

        protected override void Execute(List<GameEntity> entities) {
            foreach (var gameEntity in entities) {
                var obj = new GameObject(string.Format("entity-{0}", gameEntity.creationIndex));
                gameEntity.view.View = obj.Link(gameEntity, context);
            }
        }
    }
}