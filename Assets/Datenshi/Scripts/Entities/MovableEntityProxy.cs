using UnityEngine;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Animation;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Combat.Strategies;
using Datenshi.Scripts.Entities.Motors;
using Datenshi.Scripts.Entities.Motors.State;
using Datenshi.Scripts.Interaction;
using Datenshi.Scripts.Util;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    public class MovableEntityProxy : MonoBehaviour {
        public MovableEntity Target;

        public void Interact() {
            if (Target == null) {
                return;
            }
            Target.Interact();
        }

        public void Damage(LivingEntity entity, uint damage) {
            if (Target == null) {
                return;
            }
            Target.Damage(entity, damage);
        }

        public void Stun(float duration) {
            if (Target == null) {
                return;
            }
            Target.Stun(duration);
        }

        public void ExecuteAttack(Attack attack) {
            if (Target == null) {
                return;
            }
            Target.ExecuteAttack(attack);
        }

        public void SetInvulnerable(float seconds) {
            if (Target == null) {
                return;
            }
            Target.SetInvulnerable(seconds);
        }

        public void Kill() {
            if (Target == null) {
                return;
            }
            Target.Kill();
        }

        public void Heal(uint healthAmount) {
            if (Target == null) {
                return;
            }
            Target.Heal(healthAmount);
        }
#if UNITY_EDITOR

        public void SnapToFloor() {
            if (Target == null) {
                return;
            }
            Target.SnapToFloor();
        }
#endif

        public void RevokeOwnership() {
            if (Target == null) {
                return;
            }
            Target.RevokeOwnership();
        }

        public void SetMovementStateMachine(MotorStateMachine value) {
            if (Target == null) {
                return;
            }
            Target.MovementStateMachine = value;
        }

        public void SetMotor(Motor value) {
            if (Target == null) {
                return;
            }
            Target.Motor = value;
        }

        public void SetAccelerationCurve(AnimationCurve value) {
            if (Target == null) {
                return;
            }
            Target.AccelerationCurve = value;
        }

        public void SetAIAgent(AIAgent value) {
            if (Target == null) {
                return;
            }
            Target.AIAgent = value;
        }

        public void SetVelocity(Vector2 value) {
            if (Target == null) {
                return;
            }
            Target.Velocity = value;
        }

        public void SetSkinWidth(float value) {
            if (Target == null) {
                return;
            }
            Target.SkinWidth = value;
        }

        public void SetMaxSpeed(float value) {
            if (Target == null) {
                return;
            }
            Target.MaxSpeed = value;
        }

        public void SetYForce(float value) {
            if (Target == null) {
                return;
            }
            Target.YForce = value;
        }

        public void SetSpeedMultiplier(float value) {
            if (Target == null) {
                return;
            }
            Target.SpeedMultiplier = value;
        }

        public void SetGravityScale(float value) {
            if (Target == null) {
                return;
            }
            Target.GravityScale = value;
        }

        public void SetInteractionController(InteractionController value) {
            if (Target == null) {
                return;
            }
            Target.InteractionController = value;
        }

        public void SetApplyVelocity(bool value) {
            if (Target == null) {
                return;
            }
            Target.ApplyVelocity = value;
        }

        public void SetDamageGivesKnockback(bool value) {
            if (Target == null) {
                return;
            }
            Target.DamageGivesKnockback = value;
        }

        public void SetDamageKnockbackMin(uint value) {
            if (Target == null) {
                return;
            }
            Target.DamageKnockbackMin = value;
        }

        public void SetDamageKnockbackStrenght(float value) {
            if (Target == null) {
                return;
            }
            Target.DamageKnockbackStrenght = value;
        }

        public void SetKnockbackLiftoff(float value) {
            if (Target == null) {
                return;
            }
            Target.KnockbackLiftoff = value;
        }

        public void SetExternalForces(Vector2 value) {
            if (Target == null) {
                return;
            }
            Target.ExternalForces = value;
        }

        public void SetExternalForcesDeacceleration(float value) {
            if (Target == null) {
                return;
            }
            Target.ExternalForcesDeacceleration = value;
        }

        public void SetDamageInvulnerability(bool value) {
            if (Target == null) {
                return;
            }
            Target.DamageInvulnerability = value;
        }

        public void SetDamageInvulnerabilityDuration(float value) {
            if (Target == null) {
                return;
            }
            Target.DamageInvulnerabilityDuration = value;
        }

        public void SetRelationship(EntityRelationship value) {
            if (Target == null) {
                return;
            }
            Target.Relationship = value;
        }

        public void SetDefaultAttackStrategy(AttackStrategy value) {
            if (Target == null) {
                return;
            }
            Target.DefaultAttackStrategy = value;
        }

        public void SetOnDamaged(EntityDamagedEvent value) {
            if (Target == null) {
                return;
            }
            Target.OnDamaged = value;
        }

        public void SetOnAttack(EntityAttackEvent value) {
            if (Target == null) {
                return;
            }
            Target.OnAttack = value;
        }

        public void SetOnHealthChanged(UnityEvent value) {
            if (Target == null) {
                return;
            }
            Target.OnHealthChanged = value;
        }

        public void SetDamageGivesStun(bool value) {
            if (Target == null) {
                return;
            }
            Target.DamageGivesStun = value;
        }

        public void SetDamageStunMin(uint value) {
            if (Target == null) {
                return;
            }
            Target.DamageStunMin = value;
        }

        public void SetDamageStunDuration(float value) {
            if (Target == null) {
                return;
            }
            Target.DamageStunDuration = value;
        }

        public void SetOnKilled(UnityEvent value) {
            if (Target == null) {
                return;
            }
            Target.OnKilled = value;
        }

        public void SetDefaultAttackHitbox(Bounds2D value) {
            if (Target == null) {
                return;
            }
            Target.DefaultAttackHitbox = value;
        }

        public void SetFocusMaxTime(float value) {
            if (Target == null) {
                return;
            }
            Target.FocusMaxTime = value;
        }

        public void SetMinDefenseRequired(float value) {
            if (Target == null) {
                return;
            }
            Target.MinDefenseRequired = value;
        }

        public void SetDefenseRecoverAmountMultiplier(float value) {
            if (Target == null) {
                return;
            }
            Target.DefenseRecoverAmountMultiplier = value;
        }

        public void SetDefenseDepleteAmountMultiplier(float value) {
            if (Target == null) {
                return;
            }
            Target.DefenseDepleteAmountMultiplier = value;
        }

        public void SetHitbox(Collider2D value) {
            if (Target == null) {
                return;
            }
            Target.Hitbox = value;
        }

        public void SetAnimatorUpdater(EntityAnimatorUpdater value) {
            if (Target == null) {
                return;
            }
            Target.AnimatorUpdater = value;
        }

        public void SetRenderer(EntityRenderer value) {
            if (Target == null) {
                return;
            }
            Target.Renderer = value;
        }

        public void SetMiscController(EntityMiscController value) {
            if (Target == null) {
                return;
            }
            Target.MiscController = value;
        }

        public void SetCharacter(Character.Character value) {
            if (Target == null) {
                return;
            }
            Target.Character = value;
        }

        public void SetCurrentDirection(Direction value) {
            if (Target == null) {
                return;
            }
            Target.CurrentDirection = value;
        }

        public void SetConfig(MotorConfig value) {
            if (Target == null) {
                return;
            }
            Target.Config = value;
        }

        public void SetFocusTimeLeft(float value) {
            if (Target == null) {
                return;
            }
            Target.FocusTimeLeft = value;
        }

        public void SetDefending(bool value) {
            if (Target == null) {
                return;
            }
            Target.Defending = value;
        }

        public void SetMaxHealth(uint value) {
            if (Target == null) {
                return;
            }
            Target.MaxHealth = value;
        }

        public void SetHealthPercentage(float value) {
            if (Target == null) {
                return;
            }
            Target.HealthPercentage = value;
        }

        public void SetInvulnerable(bool value) {
            if (Target == null) {
                return;
            }
            Target.Invulnerable = value;
        }

        public void SetUseGUILayout(bool value) {
            if (Target == null) {
                return;
            }
            Target.useGUILayout = value;
        }
#if UNITY_EDITOR

        public void SetRunInEditMode(bool value) {
            if (Target == null) {
                return;
            }
            Target.runInEditMode = value;
        }
#endif

        public void SetEnabled(bool value) {
            if (Target == null) {
                return;
            }
            Target.enabled = value;
        }

        public void SetTag(string value) {
            if (Target == null) {
                return;
            }
            Target.tag = value;
        }

        public void SetName(string value) {
            if (Target == null) {
                return;
            }
            Target.name = value;
        }

        public void SetHideFlags(HideFlags value) {
            if (Target == null) {
                return;
            }
            Target.hideFlags = value;
        }
    }
}