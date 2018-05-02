using System;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;
using Rewired;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Game {
    public class PlayerInputProvider : DatenshiInputProvider {
        [SerializeField, HideInInspector]
        private uint playerID;

        [ShowInInspector, ReadOnly]
        private Player currentPlayer;

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
        public bool DebugInput;

        [ShowIf("DebugInput")]
        public float x;

        [ShowIf("DebugInput")]
        public float y;

        [ShowIf("DebugInput")]
        public bool Jump;

        [ShowIf("DebugInput")]
        public bool Attack;

        [ShowIf("DebugInput")]
        public bool Walk;

        [ShowIf("DebugInput")]
        public bool Dash;

        [ShowIf("DebugInput")]
        public bool Submit;

        [ShowIf("DebugInput")]
        public bool Defend;
#endif

        public override float GetHorizontal() {
#if UNITY_EDITOR
            if (DebugInput) {
                return x;
            }
#endif
            return Fetch(player => player.GetAxis(Actions.Horizontal));
        }

        public override float GetVertical() {
#if UNITY_EDITOR
            if (DebugInput) {
                return y;
            }
#endif
            return Fetch(player => player.GetAxis(Actions.Vertical));
        }


        public override float GetAxis(string key) {
            return currentPlayer.GetAxis(key);
        }

        public override float GetAxis(int id) {
            return currentPlayer.GetAxis(id);
        }

        public override bool GetButtonDown(string key) {
            return currentPlayer.GetButtonDown(key);
        }

        public override bool GetButtonDown(int id) {
            return currentPlayer.GetButtonDown(id);
        }

        public override bool GetButton(string key) {
            return currentPlayer.GetButton(key);
        }

        public override bool GetButton(int id) {
            return currentPlayer.GetButton(id);
        }

        public override bool GetButtonUp(string key) {
            return currentPlayer.GetButtonUp(key);
        }

        public override bool GetButtonUp(int id) {
            return currentPlayer.GetButtonUp(id);
        }

        public override bool GetJump() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Jump;
            }
#endif
            return Fetch(player => player.GetButton(Actions.Jump));
        }

        public override bool GetJumpDown() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Jump;
            }
#endif
            return Fetch(player => player.GetButtonDown(Actions.Jump));
        }

        public override bool GetAttack() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Attack;
            }
#endif
            return Fetch(player => player.GetButtonDown(Actions.Attack));
        }

        public override bool GetWalk() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Walk;
            }
#endif
            return Fetch(player => player.GetButtonDown(Actions.Walk));
        }

        public override bool GetDash() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Dash;
            }
#endif
            return Fetch(player => player.GetButtonDown(Actions.Dash));
        }

        private bool wasDefending;
        private bool canReDefend;
        private bool pressingDefend;
        public PlayerController Controller;
        private bool defendingLastFrame;

        private void Update() {
            pressingDefend = Fetch(player => player.GetButton(Actions.Defend));
            var e = Controller.CurrentEntity as LivingEntity;
            if (e != null) {
                var entityDefending = e.Focusing;
                if (!entityDefending) {
                    if (!canReDefend) {
                        canReDefend = !pressingDefend;
                    }
                } else if (defendingLastFrame && !e.CanFocus) {
                    canReDefend = false;
                }

                defendingLastFrame = e.Focusing;
            } else {
                canReDefend = true;
                defendingLastFrame = false;
            }
        }

        public override bool GetDefend() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Defend;
            }
#endif
            if (!canReDefend) {
                return false;
            }

            return pressingDefend;
        }

        public override bool GetSubmit() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Submit;
            }
#endif
            return Fetch(player => player.GetButtonDown(Actions.Submit));
        }
    }
}