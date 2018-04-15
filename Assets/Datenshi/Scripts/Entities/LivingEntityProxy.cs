using UnityEngine;
using Datenshi.Scripts.Animation;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Combat.Strategies;
using Datenshi.Scripts.Entities.Motors;
using Datenshi.Scripts.Util;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    public class LivingEntityProxy : MonoBehaviour {
        public LivingEntity Target;

        public void Stun(float duration) {
            Target.Stun(duration);
        }

        public void ExecuteAttack(Attack attack) {
            Target.ExecuteAttack(attack);
        }

        public void SetInvulnerable(float seconds) {
            Target.SetInvulnerable(seconds);
        }

        public void Kill() {
            Target.Kill();
        }

        public void Heal(uint healthAmount) {
            Target.Heal(healthAmount);
        }

        public void Damage(LivingEntity entity, uint damage) {
            Target.Damage(entity, damage);
        }
#if UNITY_EDITOR
        public void SnapToFloor() {
            Target.SnapToFloor();
        }
#endif

        public void RevokeOwnership() {
            Target.RevokeOwnership();
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

        public void SetOnHealthChanged(UnityEvent value) {
            Target.OnHealthChanged = value;
        }

        public void SetDamageGivesStun(bool value) {
            Target.DamageGivesStun = value;
        }

        public void SetDamageStunMin(uint value) {
            Target.DamageStunMin = value;
        }

        public void SetDamageStunDuration(float value) {
            Target.DamageStunDuration = value;
        }

        public void SetOnKilled(UnityEvent value) {
            Target.OnKilled = value;
        }

        public void SetDefaultAttackHitbox(Bounds2D value) {
            Target.DefaultAttackHitbox = value;
        }

        public void SetFocusMaxTime(float value) {
            Target.FocusMaxTime = value;
        }

        public void SetMinDefenseRequired(float value) {
            Target.MinDefenseRequired = value;
        }

        public void SetDefenseRecoverAmountMultiplier(float value) {
            Target.DefenseRecoverAmountMultiplier = value;
        }

        public void SetDefenseDepleteAmountMultiplier(float value) {
            Target.DefenseDepleteAmountMultiplier = value;
        }

        public void SetHitbox(Collider2D value) {
            Target.Hitbox = value;
        }

        public void SetAnimatorUpdater(EntityAnimatorUpdater value) {
            Target.AnimatorUpdater = value;
        }

        public void SetRenderer(ColorizableRenderer value) {
            Target.Renderer = value;
        }

        public void SetMiscController(EntityMiscController value) {
            Target.MiscController = value;
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

        public void SetFocusTimeLeft(float value) {
            Target.FocusTimeLeft = value;
        }

        public void SetDefending(bool value) {
            Target.Defending = value;
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

#if UNITY_EDITOR
        public void SetRunInEditMode(bool value) {
            Target.runInEditMode = value;
        }
#endif

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