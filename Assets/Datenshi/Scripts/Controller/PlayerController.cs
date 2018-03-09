using System;
using Datenshi.Input.Constants;
using Rewired;
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
            return Player.GetAxis(Actions.Horizontal);
        }

        public float GetYInput() {
            return Player.GetAxis(Actions.Vertical);
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