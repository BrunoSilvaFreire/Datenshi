using System;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;
using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif
namespace Datenshi.Scripts.Game {
    public class PlayerInputProvider : DatenshiInputProvider {
        [SerializeField, HideInInspector]
        private uint playerID;


        [ShowInInspector]
        public uint PlayerID {
            get {
                return playerID;
            }
            set {
                playerID = value;
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) {
                    return;
                }
#endif
                ReloadPlayer();
            }
        }


        [ShowInInspector, ReadOnly]
        private Player currentPlayer;

        public Player CurrentPlayer => currentPlayer;

        private void ReloadPlayer() {
            currentPlayer = ReInput.players.GetPlayer((int) playerID);
            Debug.LogFormat(
                "Using player {0} ({1}) @ id = {2}",
                currentPlayer.name,
                currentPlayer.descriptiveName,
                playerID);
        }

        private void Awake() {
            ReloadPlayer();
        }

        public T Fetch<T>(Func<Player, T> selector) {
            if (!RuntimeResources.Instance.AllowPlayerInput) {
                return default(T);
            }

            return currentPlayer == null ? default(T) : selector(currentPlayer);
        }
#if UNITY_EDITOR
        [ShowInInspector, NonSerialized]
        public bool DebugInput;

        [ShowIf("DebugInput"), ShowInInspector, NonSerialized]
        public float x;

        [ShowIf("DebugInput"), ShowInInspector, NonSerialized]
        public float y;

        [ShowIf("DebugInput"), ShowInInspector, NonSerialized]
        public bool Jump;

        [ShowIf("DebugInput"), ShowInInspector, NonSerialized]
        public bool Focus;

        [ShowIf("DebugInput"), ShowInInspector, NonSerialized]
        public bool Attack;

        [ShowIf("DebugInput"), ShowInInspector, NonSerialized]
        public bool Walk;

        [ShowIf("DebugInput"), ShowInInspector, NonSerialized]
        public bool Submit;

        [ShowIf("DebugInput"), ShowInInspector, NonSerialized]
        public bool Defend;

        [SerializeField, ReadOnly]
        private ConsumableInput jump, attack, dash;

        private void Update() {
            if (currentPlayer.GetButtonDown((int) Actions.Jump)) {
                jump.Set();
            }

            if (currentPlayer.GetButtonDown((int) Actions.Dash)) {
                dash.Set();
            }

            if (currentPlayer.GetButtonDown((int) Actions.Attack)) {
                attack.Set();
            }
        }

#endif

        public override float GetHorizontal() {
#if UNITY_EDITOR
            if (DebugInput) {
                return x;
            }
#endif
            return Fetch(player => player.GetAxis((int) Actions.Horizontal));
        }

        public override float GetVertical() {
#if UNITY_EDITOR
            if (DebugInput) {
                return y;
            }
#endif
            return Fetch(player => player.GetAxis((int) Actions.Vertical));
        }

        public override ConsumableInput GetAttack() {
            return attack;
        }

        public override bool GetDashing() {
            return Fetch(player => player.GetButton((int) Actions.Dash));
        }

        public override ConsumableInput GetDash() {
            return dash;
        }

        public override bool GetFocus() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Focus;
            }
#endif
            return Fetch(player => UnityEngine.Input.GetKey(KeyCode.LeftShift));
        }

        public override ConsumableInput GetJump() {
            return jump;
        }

        public override bool GetSubmit() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Submit;
            }
#endif
            return Fetch(player => player.GetButtonDown((int) Actions.Submit));
        }

        public override bool GetJumping() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Focus;
            }
#endif
            return Fetch(player => player.GetButton((int) Actions.Jump));
        }
    }
}