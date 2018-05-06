using System;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;
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

            var entityPos = Entity.Center;
            var navigator = Entity.AINavigator;
            var target = Target.Value;
            if (target == null) {
                return TaskStatus.Failure;
            }

            var targetCenter = target.Center;
            var targetPos = navigator == null ? targetCenter : navigator.GetFavourablePosition(target);
            var xDir = Math.Sign(entityPos.x - targetPos.x);
            var newPos = entityPos;
            var teleportDir = xDir * TeleportDistance;
            newPos.x += teleportDir;
            var room = Entity.Room;
            if (room != null && room.IsOutInBounds(newPos)) {
                newPos.x -= 2 * teleportDir;
            }

            Entity.StartCoroutine(DoTeleport(Entity, newPos));
            return TaskStatus.Running;
        }

        private IEnumerator DoTeleport(ICombatant entity, Vector2 pos) {
            teleporting = true;
            entity.Invulnerable = true;
            var updater = entity.AnimatorUpdater.Animator;
            updater.SetTrigger(StartTeleportKey);
            yield return new WaitForSeconds(TeleportDuration);
            Entity.transform.position = pos;
            updater.SetTrigger(EndTeleportKey);
            entity.Invulnerable = false;
            teleporting = false;
            finished = true;
        }
    }
}