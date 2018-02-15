using System;
using Rewired;
using UnityEngine;
using Action = Datenshi_Input_Constants.Action;
using Player = Rewired.Player;

namespace Datenshi.Scripts.Controller {
    [Serializable]
    public sealed class PlayerController : IInputProvider {
        public PlayerController(int playerId) : this(ReInput.players.GetPlayer(playerId)) { }

        public PlayerController(Player player) {
            Player = player;
        }

        public Player Player {
            get;
            private set;
        }


        public float GetXInput() {
            return Player.GetAxis(Action.Horizontal);
        }

        public float GetYInput() {
            return Player.GetAxis(Action.Vertical);
        }

        public bool GetButtonDown(int button) {
            return Player.GetButtonDown(button);
        }

        public bool GetButton(int button) {
            return Player.GetButton(button);
        }

        public bool GetButtonUp(int button) {
            return Player.GetButtonUp(button);
        }
    }
}