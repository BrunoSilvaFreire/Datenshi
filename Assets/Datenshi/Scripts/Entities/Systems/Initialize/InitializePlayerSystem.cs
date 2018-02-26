using System.Collections.Generic;
using Datenshi.Scripts.Controller;
using Datenshi.Scripts.Entities.Components.Player;
using Datenshi.Scripts.UI;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.StateMachine;
using Entitas;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Systems.Initialize {
    public class InitializePlayerSystem : IInitializeSystem {
        private readonly int[] players;
        private readonly GameContext context;
        private readonly UIContext uiContext;
        public InitializePlayerSystem(Contexts contexts, params int[] players) {
            this.players = players;
            context = contexts.game;
            uiContext = contexts.uI;
        }

        public void Initialize() {
            var mainCanvas = (RectTransform) uiContext.mainCanvas.Canvas.transform;
            var res = UIResources.Instance;
            var charSelPrefab = res.CharacterSelectionMenuPrefab;
            var viewPrefab = res.PlayerViewPrefab;
            foreach (var player in players) {
                Debug.Log("Initializing player " + player);
                var entity = context.CreateEntity();
                var controller = new PlayerController(player);
                var p = controller.Player;
                Debug.LogFormat("Has keyboard = {0}", p.controllers.hasKeyboard);
                p.controllers.hasKeyboard = true;
                var msg = string.Format("{2} ({0} - {1})", p.id, p.descriptiveName, p.name);
                Debug.LogFormat("Using controller {0} for player {1}", msg, player);
                entity.AddPlayer(
                    controller,
                    null,
                    charSelPrefab.Clone(mainCanvas),
                    viewPrefab.Clone(mainCanvas), 
                    null);
                var c = entity.player;
                c.StateMachine = new StateMachine<PlayerState, PlayerComponent>(new NormalPlayerState(), c);
            }
        }
    }
}