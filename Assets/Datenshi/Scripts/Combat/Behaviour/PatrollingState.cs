using Datenshi.Scripts.AI;
using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using UnityEngine;
using UPM.Motors;

namespace Datenshi.Scripts.Combat.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Patrolling")]
    public class PatrollingState : BehaviourState {
        public static readonly Variable<bool> Left = new Variable<bool>("entity.ai.goingLeft", true);

        public static readonly Variable<float> Distance = new Variable<float>("entity.ai.state.patrolling.distance", 0);

        public float WalkDistance = 5;
        public float MinRequiredDistance = 30;
        public BehaviourState OnSawEnemy;
        public float SightRadius = 10F;

        public override void Execute(AIStateInputProvider provider, INavigable en) {
            
            var entity = en as IMovable;
            if (entity == null) {
                return;
            }

            var c = en as ICombatant;
            if (c == null) {
                return;
            }

            if (Vector2.Distance(en.Center, Camera.main.transform.position) > MinRequiredDistance) {
                return;
            }

            provider.Walk = true;
            var left = en.GetVariable(Left);
            provider.Horizontal = left ? -1 : 1;
            var distance = en.GetVariable(Distance);
            if (distance > WalkDistance) {
                distance = 0;
                en.SetVariable(Left, !left);
            } else {
                distance += Mathf.Abs(entity.Velocity.x * Time.deltaTime);
                en.SetVariable(Distance, distance);
            }

            en.SetVariable(Distance, distance);
            DebugUtil.DrawBox2DWire(en.Center, new Vector2(SightRadius, SightRadius), Color.green);
            foreach (var hit in Physics2D.OverlapCircleAll(
                en.Center,
                SightRadius,
                GameResources.Instance.EntitiesMask)) {
                var e = hit.GetComponentInParent<ICombatant>();
                if (!c.ShouldAttack(e)) {
                    continue;
                }

                provider.CurrentState = OnSawEnemy;
                en.SetVariable(CombatVariables.AttackTarget, e);
                return;
            }
        }

        public override void DrawGizmos(AIStateInputProvider provider, INavigable entity) {
        }

        public override string GetTitle() {
            return "Patrolling State";
        }
    }
}