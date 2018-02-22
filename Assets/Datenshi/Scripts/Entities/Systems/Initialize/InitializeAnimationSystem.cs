using System.Collections.Generic;
using Datenshi.Scripts.Util;
using Entitas;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Systems.Initialize {
    public class InitializeAnimationSystem : ReactiveSystem<GameEntity> {
        public InitializeAnimationSystem(Contexts contexts) : base(contexts.game) { }


        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) {
            return context.CreateCollector(GameMatcher.Animated.Added());
        }

        protected override bool Filter(GameEntity entity) {
            return entity.hasAnimated && entity.hasView;
        }


        protected override void Execute(List<GameEntity> entities) {
            foreach (var entity in entities) {
                entity.animated.AnimatorPrefab.Clone(entity.view.View.gameObject.transform);
            }
        }
    }
}