using System.Collections.Generic;
using Datenshi.Scripts.Controller;
using Datenshi.Scripts.Entities.Components.Player;
using Datenshi.Scripts.Util.StateMachine;
using Entitas;

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
                var entity = context.CreateEntity();
                entity.AddPlayer(new PlayerController(player), null);
                var c = entity.player;
                c.StateMachine = new StateMachine<PlayerState, PlayerComponent>(c);
            }
        }
    }
}