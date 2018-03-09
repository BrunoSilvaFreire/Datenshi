using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Combat.Strategies;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public class LivingEntityProxy : MonoBehaviour {
        public LivingEntity Target;

        public void ExecuteAttack(Attack attack) {
            Target.ExecuteAttack(attack);
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

        public void SetRelationship(EntityRelationship value) {
            Target.Relationship = value;
        }

        public void SetDefaultAttackStrategy(AttackStrategy value) {
            Target.DefaultAttackStrategy = value;
        }

        public void SetOnDamaged(EntityDamagedEvent value) {
            Target.OnDamaged = value;
        }

        public void SetInputProvider(InputProvider value) {
            Target.InputProvider = value;
        }

        public void SetHitbox(Collider2D value) {
            Target.Hitbox = value;
        }

        public void SetCurrentDirection(Direction value) {
            Target.CurrentDirection = value;
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