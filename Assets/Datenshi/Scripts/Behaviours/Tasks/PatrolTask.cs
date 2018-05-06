using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Util;
using UnityEngine;

// ReSharper disable UnassignedField.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class PatrolTask : Action {
        public SharedCombatant Target;
        public MovableEntity Entity;
        public float WalkDistance = 5;
        public float SightRadius;
        public float WaitTime = 1;
        public float CloseThreshold = 1;
        private bool left;
        private bool waiting;
        private Vector2 targetPos;


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
                targetPos = Entity.GroundPosition;
                targetPos.x += (left ? -1 : 1) * WalkDistance;
                targetPos = nav.SetTarget(targetPos);

                left = !left;
                StartCoroutine(WaitInvert());
                return TaskStatus.Running;
            }

            nav.Execute(Entity, provider);

            foreach (var hit in Physics2D.OverlapCircleAll(
                Entity.Center,
                SightRadius,
                GameResources.Instance.EntitiesMask)) {
                var e = hit.GetComponentInParent<ICombatant>();
                if (!Entity.ShouldAttack(e)) {
                    continue;
                }

                Target.Value = e;
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        private IEnumerator WaitInvert() {
            waiting = true;
            yield return new WaitForSeconds(WaitTime);
            waiting = false;
        }

        public override void OnDrawGizmos() {
            DebugUtil.DrawWireCircle2D(Entity.Center, SightRadius, Color.green);
        }
    }
}