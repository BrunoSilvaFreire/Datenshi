using UnityEngine;
using System;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Combat.Strategies;
using Datenshi.Scripts.Entities.Motors;
using Datenshi.Scripts.Entities.Motors.State;
using Datenshi.Scripts.Util;

namespace Datenshi.Scripts.Entities {
    public class MovableEntityProxy : MonoBehaviour {
        public MovableEntity Target;

        public void ExecuteAttack(Attack attack) {
            Target.ExecuteAttack(attack);
        }

        public void SetInvulnerable(float seconds) {
            Target.SetInvulnerable(seconds);
        }

        public void Kill() {
            Target.Kill();
        }

        public void Damage(LivingEntity entity, uint damage) {
            Target.Damage(entity, damage);
        }

        public void SnapToFloor() {
            Target.SnapToFloor();
        }

        public void SetMovementStateMachine(MotorStateMachine value) {
            Target.MovementStateMachine = value;
        }

        public void SetMotor(Motor value) {
            Target.Motor = value;
        }

        public void SetAccelerationCurve(AnimationCurve value) {
            Target.AccelerationCurve = value;
        }

        public void SetAiAgent(AIAgent value) {
            Target.aiAgent = value;
        }

        public void SetVelocity(Vector2 value) {
            Target.Velocity = value;
        }

        public void SetSkinWidth(float value) {
            Target.SkinWidth = value;
        }

        public void SetMaxSpeed(float value) {
            Target.MaxSpeed = value;
        }

        public void SetYForce(float value) {
            Target.YForce = value;
        }

        public void SetSpeedMultiplier(float value) {
            Target.SpeedMultiplier = value;
        }

        public void SetGravityScale(float value) {
            Target.GravityScale = value;
        }

        public void SetDamageInvulnerability(bool value) {
            Target.DamageInvulnerability = value;
        }

        public void SetDamageInvulnerabilityDuration(float value) {
            Target.DamageInvulnerabilityDuration = value;
        }

        public void SetRelationship(EntityRelationship value) {
            Target.Relationship = value;
        }

        public void SetDefaultAttackStrategy(AttackStrategy value) {
            Target.DefaultAttackStrategy = value;
        }

        public void SetOnDamaged(EntityDamagedEvent value) {
            Target.OnDamaged = value;
        }

        public void SetOnAttack(EntityAttackEvent value) {
            Target.OnAttack = value;
        }

        public void SetHitbox(Collider2D value) {
            Target.Hitbox = value;
        }

        public void SetCharacter(Character.Character value) {
            Target.Character = value;
        }

        public void SetCurrentDirection(Direction value) {
            Target.CurrentDirection = value;
        }

        public void SetConfig(MotorConfig value) {
            Target.Config = value;
        }

        public void SetMaxHealth(uint value) {
            Target.MaxHealth = value;
        }

        public void SetHealthPercentage(float value) {
            Target.HealthPercentage = value;
        }

        public void SetInvulnerable(bool value) {
            Target.Invulnerable = value;
        }

        public void SetUseGUILayout(bool value) {
            Target.useGUILayout = value;
        }

        public void SetRunInEditMode(bool value) {
            Target.runInEditMode = value;
        }

        public void SetEnabled(bool value) {
            Target.enabled = value;
        }

        public void SetTag(string value) {
            Target.tag = value;
        }

        public void SetName(string value) {
            Target.name = value;
        }

        public void SetHideFlags(HideFlags value) {
            Target.hideFlags = value;
        }
    }
}