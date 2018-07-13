using UnityEngine;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Util;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    public class LivingEntityProxy : MonoBehaviour {
        public LivingEntity Target;

        public void Stun(float duration) {
            Target.Stun(duration);
        }

        public void ExecuteAttack(Attack attack) {
            Target.ExecuteSkill(attack);
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
        public void RevokeOwnership() {
            Target.RevokeOwnership();
        }

        public void SetDamageInvulnerability(bool value) {
            Target.DamageInvulnerability = value;
        }

        public void SetDamageInvulnerabilityDuration(float value) {
            Target.DamageInvulnerabilityDuration = value;
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


        public void SetFocusMaxTime(float value) {
            Target.FocusMaxTime = value;
        }

        public void SetMinDefenseRequired(float value) {
            Target.MinFocusRequired = value;
        }

        public void SetDefenseRecoverAmountMultiplier(float value) {
            Target.FocusRecoverAmountMultiplier = value;
        }

        public void SetDefenseDepleteAmountMultiplier(float value) {
            Target.FocusDepleteAmountMultiplier = value;
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

        public void SetFocusTimeLeft(float value) {
            Target.FocusTimeLeft = value;
        }


        public void SetMaxHealth(uint value) {
            Target.MaxHealth = value;
        }

        public void SetHealthPercentage(float value) {
            Target.HealthPercentage = value;
        }

        public void SetInvulnerable(bool value) {
            Target.GodMode = value;
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