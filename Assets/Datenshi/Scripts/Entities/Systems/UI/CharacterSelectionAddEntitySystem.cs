using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Character;
using Entitas;

namespace Datenshi.Scripts.Entities.Systems.UI {
    public class CharacterSelectionAddEntitySystem : ReactiveSystem<GameEntity> {
        public CharacterSelectionAddEntitySystem(Contexts contexts) : base(contexts.game) {
            players = contexts.game.CreateCollector(GameMatcher.Player);
        }

        private ICollector<GameEntity> players;

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) {
            return context.CreateCollector(GameMatcher.Character.Added());
        }

        protected override bool Filter(GameEntity entity) {
            return entity.character.Character is PlayableCharacter;
        }

        protected override void Execute(List<GameEntity> entities) {
            var ps = from e in players.collectedEntities select e.player;
            foreach (var player in ps) {
                foreach (var entity in entities) {
                    player.CharacterSelectionMenu.AddCharacter(player, entity);
                }
            }
        }
    }
}