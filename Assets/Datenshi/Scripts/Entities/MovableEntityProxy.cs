using UnityEngine;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Util;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    public class MovableEntityProxy : MonoBehaviour {
        public MovableEntity Target;

        public void AddXImpulse(float force) {
            if (Target == null) {
                return;
            }

            Target.ExternalForces += new Vector2(Target.CurrentDirection.X * force, 0);
        }

        public void AddYImpulse(float force) {
            if (Target == null) {
                return;
            }

            Target.ExternalForces += new Vector2(0, Target.CurrentDirection.Y * force);
        }
        public void Stun(float duration) {
            if (Target == null) {
                return;
            }

            Target.Stun(duration);
        }

        public void ExecuteSkill(ActiveSkill attack) {
            if (Target == null) {
                return;
            }

            Target.ExecuteSkill(attack);
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

        public void RevokeOwnership() {
            if (Target == null) {
                return;
            }

            Target.RevokeOwnership();
        }

        public void SetAIAgent(AINavigator value) {
            if (Target == null) {
                return;
            }

            Target.AINavigator = value;
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

            Target.SpeedMultiplier.BaseValue = value;
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

        public void SetDamageInvulnerability(int value) {
            if (Target == null) {
                return;
            }

            Target.DamageInvulnerability = value == 1;
        }

        public void SetDamageInvulnerabilityDuration(float value) {
            if (Target == null) {
                return;
            }

            Target.DamageInvulnerabilityDuration = value;
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

            Target.MinFocusRequired = value;
        }

        public void SetDefenseRecoverAmountMultiplier(float value) {
            if (Target == null) {
                return;
            }

            Target.FocusRecoverAmountMultiplier = value;
        }

        public void SetDefenseDepleteAmountMultiplier(float value) {
            if (Target == null) {
                return;
            }

            Target.FocusDepleteAmountMultiplier = value;
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

        public void SetFocusTimeLeft(float value) {
            if (Target == null) {
                return;
            }

            Target.FocusTimeLeft = value;
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

            Target.GodMode = value;
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