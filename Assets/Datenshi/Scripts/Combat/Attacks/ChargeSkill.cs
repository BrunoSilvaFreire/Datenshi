using Datenshi.Scripts.Movement;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    [CreateAssetMenu(menuName = "Datenshi/Skill/Charge")]
    public class ChargeSkill : ActiveSkill {
        public bool TowardsCombatTarget;

        [ShowIf(nameof(TowardsCombatTarget))]
        public float Magnitude;

        [HideIf(nameof(TowardsCombatTarget))]
        public Vector2 Velocity = Vector2.right;

        public override void Execute(ICombatant entity) {
            var m = entity as IDatenshiMovable;
            if (m == null) {
                return;
            }

            m.ExternalForces = GetVelocity(entity);
            Debug.Log("Velocity = " + m.ExternalForces);
        }

        private Vector2 GetVelocity(ICombatant entity) {
            Vector2 vel;
            if (TowardsCombatTarget) {
                var t = entity.GetVariable(CombatVariables.AttackTarget);
                if (t == null) {
                    Debug.LogWarning("No combat target! " + entity, this);
                    return Vector2.zero;
                }

                vel = t.Transform.position - entity.Transform.position;
                vel.Normalize();
                vel *= Magnitude;
            } else {
                vel = Velocity;
                vel.x *= entity.CurrentDirection.X;
            }

            return vel;
        }
    }
}