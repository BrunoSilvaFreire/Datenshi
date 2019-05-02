using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Lunari.Tsuki;
using Lunari.Tsuki.Misc;
using UnityEngine;

// ReSharper disable UnassignedField.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class PatrolTask : Action {
        public enum PatrolState {
            Fail,
            Running,
            Waiting
        }

        public SharedCombatant Target;
        public MovableEntity Entity;
        public float WalkDistance = 5;
        public Bounds2D SightRadius;
        public float WaitTime = 1;
        public float CloseThreshold = 1;
        public bool StartAtLeft = true;
        private bool left;
        private bool waiting;
        private Vector2 targetPos;

        public override void OnStart() {
            left = StartAtLeft;
            RecalculateTargetPos(Entity.AINavigator);
        }

        public override TaskStatus OnUpdate() {
            var provider = Entity.InputProvider as DummyInputProvider;
            if (provider == null) {
                Debug.LogError($"There is no DummyProvider on Entity {Entity}");
                return TaskStatus.Failure;
            }

            //provider.Walk = true;
            var xInput = left ? -1 : 1;
            provider.Horizontal = xInput;
            var nav = Entity.AINavigator;
            var d = Vector2.Distance(Entity.GroundPosition, targetPos);
            if (d < CloseThreshold || Entity.CollisionStatus.HorizontalCollisionDir != 0) {
                left = !left;
                RecalculateTargetPos(nav);
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
            targetPos = nav.GetFavourableStartPosition(Entity);
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

                valid = nav.IsValid(node);
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
            Debugging.DrawWireBox2D(Entity.Center + Entity.CurrentDirection.X * SightRadius.Center, SightRadius.Size,
                Color.red);
            var color = waiting ? Color.yellow : Color.green;
            Debugging.DrawWireBox2D(targetPos, Vector2.one, color);
            Debug.DrawLine(Entity.GroundPosition, targetPos, color);
        }
    }
}