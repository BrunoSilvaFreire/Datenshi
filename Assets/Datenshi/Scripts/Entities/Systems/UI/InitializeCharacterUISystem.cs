using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Character;
using Datenshi.Scripts.Controller;
using Datenshi.Scripts.Entities.Components.Player;
using Datenshi.Scripts.Util;
using Entitas;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Systems.UI {
    public class CharacterSelectionAddEntitySystem : ReactiveSystem<GameEntity> {
        public CharacterSelectionAddEntitySystem(Contexts contexts) : base(contexts.game) {
            players = contexts.game.CreateCollector(GameMatcher.Player);
        }

        private ICollector<GameEntity> players;

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) {
            return context.CreateCollector(GameMatcher.Character);
        }

        protected override bool Filter(GameEntity entity) {
            return entity.character.Character is PlayableCharacter;
        }

        protected override void Execute(List<GameEntity> entities) {
            var ps = from e in players.collectedEntities select e.player;
            var playerComponents = ps as PlayerComponent[] ?? ps.ToArray();
            foreach (var entity in entities) {
                var character = entity.character.Character;
                var playableCharacter = character as PlayableCharacter;
                if (playableCharacter != null && entity.hasView) {
                    var view = entity.view.View.gameObject.transform;
                    var camera = GameResources.Instance.CharacterCameraPrefab.Clone(view);
                    camera.transform.localPosition = Vector3.back;
                    camera.targetTexture = playableCharacter.CharacterScreen;
                }
                foreach (var player in playerComponents) {
                    var menu = player.CharacterSelectionMenu;
                    menu.AddCharacter(player, entity);
                    menu.SnapShowing(false);
                }
            }
        }
    }
}