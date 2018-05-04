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
        public float MinRequiredDistance = 20;
        public float WalkDistance = 5;
        public float SightRadius;
        private float distance;
        private bool left;

        public override TaskStatus OnUpdate() {
            if (Target.Value != null) {
                return TaskStatus.Success;
            }

            var m = Camera.main;
            var provider = Entity.InputProvider as DummyInputProvider;
            if (provider == null || Vector2.Distance(Entity.Center, m.transform.position) > MinRequiredDistance) {
                return TaskStatus.Failure;
            }

            provider.Walk = true;
            provider.Horizontal = left ? -1 : 1;
            if (distance > WalkDistance) {
                distance = 0;
                left = !left;
            } else {
                distance += Mathf.Abs(Entity.Velocity.x * Time.deltaTime);
            }

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

        public override void OnDrawGizmos() {
            var m = Camera.main;
            var a = Entity.Center;
            var b = m.transform.position;
            var d = Vector2.Distance(a, b);
            var c = d > MinRequiredDistance ? Color.red : Color.green;
            DebugUtil.DrawBox2DWire(Entity.Center, new Vector2(SightRadius, SightRadius), Color.green);
            Debug.DrawLine(a, b, c);
        }
    }
}