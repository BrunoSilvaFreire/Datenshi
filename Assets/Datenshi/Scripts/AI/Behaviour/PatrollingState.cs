using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Debugging;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Patrolling")]
    public class PatrollingState : BehaviourState {
        public static readonly Variable<bool> Left = new Variable<bool>("entity.ai.goingLeft", true);

        public static readonly Variable<float> Distance = new Variable<float>("entity.ai.state.patrolling.distance", 0);

        public float WalkDistance = 5;
        public float MinRequiredDistance = 30;
        public BehaviourState OnSawEnemy;
        public float SightRadius = 10F;

        public override void Execute(AIStateInputProvider provider, Entity en, DebugInfo info) {
            var playerEntity = PlayerController.Instance.CurrentEntity;
            if (playerEntity == null) {
                return;
            }

            var entity = en as MovableEntity;
            if (entity == null) {
                return;
            }

            if (Vector2.Distance(playerEntity.transform.position, entity.transform.position) > MinRequiredDistance) {
                return;
            }

            provider.Walk = true;
            var left = entity.GetVariable(Left);
            provider.Horizontal = left ? -1 : 1;
            var distance = entity.GetVariable(Distance);
            if (distance > WalkDistance) {
                distance = 0;
                entity.SetVariable(Left, !left);
            } else {
                distance += Mathf.Abs(entity.Velocity.x * Time.deltaTime);
                entity.SetVariable(Distance, distance);
            }

            entity.SetVariable(Distance, distance);
            DebugUtil.DrawBox2DWire(entity.GroundPosition, new Vector2(SightRadius, SightRadius), Color.green);
            foreach (var hit in Physics2D.OverlapCircleAll(
                entity.GroundPosition,
                SightRadius,
                GameResources.Instance.EntitiesMask)) {
                var e = hit.GetComponentInParent<LivingEntity>();
                if (!entity.ShouldAttack(e)) {
                    continue;
                }

                provider.CurrentState = OnSawEnemy;
                entity.SetVariable(CombatVariables.EntityTarget, e);
                return;
            }
        }

        public override void DrawGizmos(AIStateInputProvider provider, Entity entity, DebugInfo info) {
            CombatDebug.DrawCombatInfo(entity, info);
        }

        public override string GetTitle() {
            return "Patrolling State";
        }
    }
}