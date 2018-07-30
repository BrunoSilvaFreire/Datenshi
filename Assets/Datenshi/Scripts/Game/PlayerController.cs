using System;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game.Rank;
using Datenshi.Scripts.Game.Restart;
using Datenshi.Scripts.Game.Time;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Singleton;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Datenshi.Scripts.Game {
    [Serializable]
    public class PlayerEntityChangedEvent : UnityEvent<Entity, Entity> { }

    [Serializable]
    public class PlayerRankXPGainedEvent : UnityEvent<float> { }


    public class PlayerController : Singleton<PlayerController>, IRestartable {
        public PlayerInputProvider Player;

        [SerializeField, HideInInspector]
        private Entity currentEntity;

        public Rank.Rank Rank;


        public PlayerEntityChangedEvent OnEntityChanged;
        public PlayerRankXPGainedEvent PlayerRankXPGainedEvent;

        [ShowInInspector]
        public Entity CurrentEntity {
            get {
                return currentEntity;
            }
            set {
                if (currentEntity != null) {
                    currentEntity.RevokeOwnership();
                }

                var old = currentEntity;
                currentEntity = value;
                OnEntityChanged.Invoke(old, value);
                currentEntity.ForceRequestOwnership(Player);
            }
        }

        public Canvas Canvas;

        public PlayableDirector Director;


        private void Start() {
            if (currentEntity.InputProvider != Player) {
                currentEntity.RevokeOwnership();
                currentEntity.RequestOwnership(Player);
            }

            OnEntityChanged.Invoke(null, currentEntity);
            GlobalEntityDamagedEvent.Instance.AddListener(OnEntityDamaged);
        }

        private void OnEntityDamaged(LivingEntity damaged, IDamageDealer damager, IDamageSource damageSource,
            uint damage) {
            var attack = damageSource as Attack;
            if (attack == null) {
                return;
            }

            if (currentEntity != null && (Entity) damager == currentEntity) {
                HandleRankAttack(attack);
            }
        }

        private Attack lastAttack;
        private uint timesReused;
        public float RankXPGainedWaitDuration = 2;
        public float RankXPDropSpeed = .1F;
        private float xpStopDurationLeft;


        private void HandleRankAttack(Attack attack) {
            if (attack == lastAttack) {
                timesReused++;
            } else {
                timesReused = 0;
                lastAttack = attack;
            }

            var xpToWin = GameResources.Instance.RankXPGraph.Evaluate(timesReused);
            if (xpToWin <= 0) {
                return;
            }

            xpStopDurationLeft = RankXPGainedWaitDuration;
            Rank.XP += xpToWin;
            PlayerRankXPGainedEvent.Invoke(xpToWin);
        }

        private void Update() {
            UpdateRank();
            UpdatePause();
        }

        private void UpdatePause() {
            if (Player.GetButtonDown((int) Actions.Cancel)) {
                RuntimeResources.Instance.TogglePaused();
            }
        }


        private void UpdateRank() {
            var delta = UnityEngine.Time.deltaTime;
            if (xpStopDurationLeft > 0) {
                xpStopDurationLeft -= delta;
                return;
            }

            var toDrop = RankXPDropSpeed * delta;
            if (Rank.CurrentLevel > RankLevel.F || Rank.XP > toDrop) {
                Rank.XP -= toDrop;
            } else {
                Rank.XP = 0;
            }
        }

        public static Entity GetOrCreateEntity() {
            var player = Instance;
            if (player == null) {
                Debug.LogWarning("Couldn't find player controller in scene, loading from singletons...");
                player = Singletons.Instance.PlayerControllerPrefab.Clone();
            }

            return player.CurrentEntity;
        }

        public void Restart() {
            var le = CurrentEntity as LivingEntity;
            if (le == null) {
                return;
            }

            le.Heal();
        }
    }
}