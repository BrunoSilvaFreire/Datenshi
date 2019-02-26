using System;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Game.Restart;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Misc;
using Lunari.Tsuki;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Game {
    public class SpawnPoint : AbstractRoomMember, IRestartable {
        public enum PositionSourceType {
            Player,
            SpawnPoint
        }

        [Required]
        public Entity Prefab;

        public PositionSourceType PositionSource;
        public DistanceThreshold Threshold = new DistanceThreshold(15, 20);
        public AnimationCurve DistanceMultiplier = AnimationCurve.EaseInOut(0, 1, 3, 2);
        private uint totalKills;

        [ShowInInspector]
        private Entity active;

        [ShowInInspector]
        private bool killed;

        private void Update() {
            var player = PlayerController.Instance.CurrentEntity;
            if (player == null) {
                return;
            }

            var pPos = player.Center;
            Vector2 sourcePos;
            switch (PositionSource) {
                case PositionSourceType.Player:
                    sourcePos = pPos;
                    break;
                case PositionSourceType.SpawnPoint:
                    sourcePos = transform.position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            bool withinMaximum;
            if (active == null) {
                if (killed) {
                    withinMaximum = Threshold.IsWithinMaximum(transform, pPos, DistanceMultiplier.Evaluate(totalKills));
                    if (!withinMaximum) {
                        killed = false;
                    }

                    return;
                }

                if (Threshold.IsWithinMinimum(transform, pPos)) {
                    Spawn();
                }
            } else {
                withinMaximum = Threshold.IsWithinMaximum(active.Center, sourcePos);
                if (withinMaximum) {
                    return;
                }

                if (!killed) {
                    Despawn();
                }
            }
        }

        private void Despawn() {
            Destroy(active.gameObject);
            active = null;
        }

        private void Spawn() {
            if (Prefab == null) {
                return;
            }

            active = Instantiate(Prefab, transform.position, Quaternion.identity);
            killed = false;

            var l = active as LivingEntity;
            if (l == null) {
                return;
            }

            if (Room != null) {
                Room.AddMember(l);
            }

            l.OnKilled.AddListener(OnKilled);
        }

        private void OnKilled() {
            killed = true;
            totalKills++;
            ((LivingEntity) active).OnKilled.RemoveListener(OnKilled);
        }

        private void OnDrawGizmos() {
            GizmosUtility.DrawWireCircle2D(transform.position, 1f, Color.red);
        }

        private void OnDrawGizmosSelected() {
            Threshold.DrawGizmos(transform.position, DistanceMultiplier);
        }

        public void Restart() {
            totalKills = 0;
        }
    }
}