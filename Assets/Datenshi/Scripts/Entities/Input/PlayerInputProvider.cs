﻿using System;
using Datenshi.Input.Constants;
using Datenshi.Scripts.Game;
using Rewired;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Input {
    public class PlayerInputProvider : InputProvider {
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
            if (!RuntimeVariables.Instance.AllowPlayerInput) {
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
#endif
        public bool GetPlanningMenu() {
            return Fetch(player => player.GetButton(Actions.Planning));
        }

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

        public override bool GetJump() {
#if UNITY_EDITOR
            if (DebugInput) {
                return Jump;
            }
#endif
            return Fetch(player => player.GetButton(Actions.Jump));
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
    }
}