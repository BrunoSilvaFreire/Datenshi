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

        public InitializePlayerSystem(Contexts contexts, params int[] players) {
            this.players = players;
            context = contexts.game;
        }

        public void Initialize() {
            foreach (var player in players) {
                Debug.Log("Initializing player " + player);
                var entity = context.CreateEntity();
                entity.AddPlayer(new PlayerController(player), null,
                    UIResources.Instance.CharacterSelectionMenuPrefab.Clone());
                var c = entity.player;
                c.StateMachine = new StateMachine<PlayerState, PlayerComponent>(new NormalPlayerState(), c);
            }
        }
    }
}