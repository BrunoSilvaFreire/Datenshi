using Datenshi.Scripts.Controller;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Origame/AI/States/Patrolling")]
    public class PatrollingState : BehaviourState {
        public enum StartDirection {
            Left,
            Right
        }

        public static readonly Variable<bool> Left = new Variable<bool>("entity.ai.goingLeft", true);

        public static readonly Variable<float> Distance = new Variable<float>("entity.ai.state.patrolling.distance", 0);

        public const float WalkDistance = 5;
        public BehaviourState OnSawEnemy;
        public float SightRadius = 10F;

        public override void Execute(AIStateInputProvider provider, MovableEntity entity) {
            provider.Walk = true;
            var left = entity.GetVariable(Left);
            provider.Horizontal = left ? -1 : 1;
            var distance = entity.GetVariable(Distance);
            if (distance > WalkDistance) {
                distance = 0;
                entity.SetVariable(Left, !left);
            } else {
                distance += entity.Velocity.x;
            }

            entity.SetVariable(Distance, distance);
            DebugUtil.DrawBox2DWire(entity.GroundPosition, new Vector2(SightRadius, SightRadius), Color.green);
            foreach (var hit in Physics2D.OverlapCircleAll(
                entity.GroundPosition,
                SightRadius,
                GameResources.Instance.EntitiesMask)) {
                var e = hit.GetComponentInParent<LivingEntity>();
                if (e == null || e == entity || e.Relationship == entity.Relationship) {
                    continue;
                }

                provider.CurrentState = OnSawEnemy;
                entity.SetVariable(AttackingState.EntityTarget, e);
                return;
            }
        }
    }
}