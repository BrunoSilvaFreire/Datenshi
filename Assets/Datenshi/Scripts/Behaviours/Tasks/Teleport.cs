using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.World.Rooms;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class Teleport : Action {
        public DummyInputProvider Provider;
        public MovableEntity Entity;
        public SharedCombatant Target;
        private bool teleporting;
        public float TeleportDuration = 5;
        public float TeleportDistance = 7;
        public string TeleportingKey = "Teleporting";
        public string StartTeleportKey = "TeleportBegin";
        public string EndTeleportKey = "TeleportEnd";
        private bool finished;

        public override TaskStatus OnUpdate() {
            if (teleporting) {
                Provider.Reset();
                return TaskStatus.Running;
            }

            if (finished) {
                finished = false;
                return TaskStatus.Success;
            }

            var target = Target.Value;
            if (target == null) {
                return TaskStatus.Failure;
            }

            Entity.StartCoroutine(DoTeleport(Entity, target));
            return TaskStatus.Running;
        }

        private Vector2 GetBestPosition(Vector2 targetPos) {
            var entityPos = Entity.Center;
            var left = entityPos;
            var right = entityPos;
            left.x -= TeleportDistance;
            right.x += TeleportDistance;
            ClampToRoom(ref left, entityPos, Entity);
            ClampToRoom(ref right, entityPos, Entity);
            var da = Vector2.Distance(targetPos, left);
            var db = Vector2.Distance(targetPos, right);
            return da > db ? left : right;
        }

        private static void ClampToRoom(ref Vector2 pos, Vector2 original, IRoomMember entity) {
            var mask = GameResources.Instance.WorldMask;
            var linecast = Physics2D.Linecast(original, pos, mask);
            if (!linecast) {
                return;
            }

            var dir = pos - original;
            dir.Normalize();
            dir *= linecast.distance - .1F;
            pos = original + dir;
            var room = entity.Room;
            if (room != null && room.IsOutInBounds(pos)) {
                ClampToRoom(ref pos, room);
            }
        }

        private static void ClampToRoom(ref Vector2 newPos, Room room) {
            var bounds = room.Area.bounds;
            var min = bounds.min;
            var max = bounds.max;
            if (newPos.x > max.x) {
                newPos.x = max.x;
            }

            if (newPos.y > max.y) {
                newPos.y = max.y;
            }

            if (newPos.x < min.x) {
                newPos.x = min.x;
            }

            if (newPos.y < min.y) {
                newPos.y = min.y;
            }
        }


        private IEnumerator DoTeleport(ICombatant entity, ILocatable target) {
            teleporting = true;
            entity.GodMode = true;
            var updater = entity.AnimatorUpdater.Animator;
            updater.SetTrigger(StartTeleportKey);
            updater.SetBool(TeleportingKey, true);
            yield return new WaitForSeconds(TeleportDuration);
            Entity.transform.position = GetBestPosition(target.Center);
            updater.SetTrigger(EndTeleportKey);
            entity.GodMode = false;
            updater.SetBool(TeleportingKey, false);
            teleporting = false;
            finished = true;
        }
    }
}