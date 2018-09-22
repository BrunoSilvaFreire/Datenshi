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
        public bool Dash;

        [ShowIf("DebugInput"), ShowInInspector, NonSerialized]
        public bool Submit;

        [ShowIf("DebugInput"), ShowInInspector, NonSerialized]
        public bool Defend;
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

        public override bool GetFocus() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Focus;
            }
#endif
            return Fetch(player => player.GetButton((int) Actions.Focus));
        }

        public override float GetAxis(string key) {
            if (!RuntimeResources.Instance.AllowPlayerInput) {
                return 0;
            }

            return currentPlayer.GetAxis(key);
        }

        public override float GetAxis(int id) {
            if (!RuntimeResources.Instance.AllowPlayerInput) {
                return 0;
            }

            return currentPlayer.GetAxis(id);
        }

        public override bool GetButtonDown(string key) {
            return RuntimeResources.Instance.AllowPlayerInput && currentPlayer.GetButtonDown(key);
        }

        public override bool GetButtonDown(int id) {
            return RuntimeResources.Instance.AllowPlayerInput && currentPlayer.GetButtonDown(id);
        }

        public override bool GetButton(string key) {
            return RuntimeResources.Instance.AllowPlayerInput && currentPlayer.GetButton(key);
        }

        public override bool GetButton(int id) {
            return RuntimeResources.Instance.AllowPlayerInput && currentPlayer.GetButton(id);
        }

        public override bool GetButtonUp(string key) {
            return RuntimeResources.Instance.AllowPlayerInput && currentPlayer.GetButtonUp(key);
        }

        public override bool GetButtonUp(int id) {
            return RuntimeResources.Instance.AllowPlayerInput && currentPlayer.GetButtonUp(id);
        }


        public override bool GetJump() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Jump;
            }
#endif
            return Fetch(player => player.GetButton((int) Actions.Jump));
        }

        public override bool GetJumpDown() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Jump;
            }
#endif
            return Fetch(player => player.GetButtonDown((int) Actions.Jump));
        }

        public override bool GetAttack() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Attack;
            }
#endif
            return Fetch(player => player.GetButtonDown((int) Actions.Attack));
        }


        public override bool GetDash() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Dash;
            }
#endif
            return Fetch(player => player.GetButtonDown((int) Actions.Dash));
        }


        public override bool GetDefend() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Defend;
            }
#endif

            return Fetch(player => player.GetButton((int) Actions.Defend));
        }

        public override bool GetSubmit() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Submit;
            }
#endif
            return Fetch(player => player.GetButtonDown((int) Actions.Submit));
        }
    }
}