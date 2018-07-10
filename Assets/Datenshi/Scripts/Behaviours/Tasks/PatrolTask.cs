using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Util;
using UnityEngine;
using UPM.Util;

// ReSharper disable UnassignedField.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class PatrolTask : Action {
        public SharedCombatant Target;
        public MovableEntity Entity;
        public float WalkDistance = 5;
        public Bounds2D SightRadius;
        public float WaitTime = 1;
        public float CloseThreshold = 1;
        private bool left;
        private bool waiting;

        public Vector2 targetPos;

        public override void OnStart() {
            RecalculateTargetPos(Entity.AINavigator);
        }

        public override TaskStatus OnUpdate() {
            if (Target.Value != null) {
                return TaskStatus.Success;
            }


            var provider = Entity.InputProvider as DummyInputProvider;
            if (provider == null) {
                return TaskStatus.Failure;
            }

            if (waiting) {
                provider.Reset();
                return TaskStatus.Running;
            }

            provider.Walk = true;
            var xInput = left ? -1 : 1;
            provider.Horizontal = xInput;
            var nav = Entity.AINavigator;
            var d = Vector2.Distance(Entity.GroundPosition, targetPos);
            if (d < CloseThreshold || Entity.CollisionStatus.HorizontalCollisionDir != 0) {
                RecalculateTargetPos(nav);
                left = !left;
                StartCoroutine(WaitInvert());
                return TaskStatus.Running;
            }

            nav.Execute(Entity, provider);
            var hits = Physics2D.OverlapBoxAll(
                Entity.Center + Entity.CurrentDirection.X * SightRadius.Center,
                SightRadius.Size,
                GameResources.Instance.EntitiesMask);
            foreach (var hit in hits) {
                var e = hit.GetComponentInParent<ICombatant>();
                if (!Entity.ShouldAttack(e)) {
                    continue;
                }

                Target.Value = e;
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        private const int MaxFixTries = 10;

        private void RecalculateTargetPos(AINavigator nav) {
            targetPos = Entity.GroundPosition;
            var dir = left ? -1 : 1;
            targetPos.x += dir * WalkDistance;
            targetPos = nav.SetTarget(targetPos);
            var mesh = Navmesh.Instance;
            bool valid;
            var currentTry = 0;
            do {
                var node = mesh.GetNodeAtWorld(targetPos);
                if (node == null) {
                    break;
                }

                valid = !node.IsBlocked;
                if (!valid) {
                    targetPos.x -= dir;
                }

                
                if (++currentTry > MaxFixTries) {
                    break;
                }
            } while (!valid);
        }

        private IEnumerator WaitInvert() {
            waiting = true;
            yield return new WaitForSeconds(WaitTime);
            waiting = false;
        }

        public override void OnDrawGizmos() {
            DebugUtil.DrawBox2DWire(Entity.Center + Entity.CurrentDirection.X * SightRadius.Center, SightRadius.Size,
                Color.red);
            DebugUtil.DrawBox2DWire(targetPos, Vector2.one, Color.magenta);
            Debug.DrawLine(Entity.GroundPosition, targetPos, Color.magenta);
        }
    }
}