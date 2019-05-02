using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    public class MovableEntityProxy : MonoBehaviour {
        public MovableEntity Target;

        public void ExecuteSkill(ActiveSkill skill) {
            Target.ExecuteSkill(skill);
        }

        public void BreakDefense() {
            Target.BreakDefense();
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

        public void Heal() {
            Target.Heal();
        }

        public void Stun(float duration) {
            Target.Stun(duration);
        }

        public void RevokeOwnership() {
            Target.RevokeOwnership();
        }

        public void ForceRequestOwnership(DatenshiInputProvider player) {
            Target.ForceRequestOwnership(player);
        }

        public void SetRigidbody(Rigidbody2D value) {
            if (Target == null) {
                return;
            }

            Target.Rigidbody = value;
        }

        public void SetRigidStateHolder(GameObject value) {
            if (Target == null) {
                return;
            }

            Target.RigidStateHolder = value;
        }

        public void SetSpeedMultiplier(float value) {
            if (Target == null) {
                return;
            }

            Target.SpeedMultiplier.BaseValue = value;
        }

        public void SetDirectionChangeThreshold(float value) {
            if (Target == null) {
                return;
            }

            Target.DirectionChangeThreshold = value;
        }

        public void SetOutlineInvulnerabilityMinSecondsLeft(float value) {
            if (Target == null) {
                return;
            }

            Target.OutlineInvulnerabilityMinSecondsLeft = value;
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

        public void SetOnAttack(EntityAttackEvent value) {
            if (Target == null) {
                return;
            }

            Target.OnAttack = value;
        }
        public void SetMinFocusRequired(float value) {
            if (Target == null) {
                return;
            }

            Target.MinDefenseRequired = value;
        }

        public void SetOnHealthChanged(UnityEvent value) {
            if (Target == null) {
                return;
            }

            Target.OnHealthChanged = value;
        }

        public void SetDamageColor(Color value) {
            if (Target == null) {
                return;
            }

            Target.DamageColor = value;
        }

        public void SetDamageColorAmount(float value) {
            if (Target == null) {
                return;
            }

            Target.DamageColorAmount = value;
        }

        public void SetDefenseBreakStunDuration(float value) {
            if (Target == null) {
                return;
            }

            Target.DefenseBreakStunDuration = value;
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

        public void SetIgnored(bool value) {
            if (Target == null) {
                return;
            }

            Target.Ignored = value;
        }


        public void SetDefending(bool value) {
            if (Target == null) {
                return;
            }

            Target.Defending = value;
        }

        public void SetHealthPercentage(float value) {
            if (Target == null) {
                return;
            }

            Target.HealthPercentage = value;
        }

        public void SetGodMode(bool value) {
            if (Target == null) {
                return;
            }

            Target.GodMode = value;
        }

        public void SetMaxHealth(uint value) {
            if (Target == null) {
                return;
            }

            Target.MaxHealth = value;
        }

        public void SetTimeScaleIndependent(bool value) {
            if (Target == null) {
                return;
            }

            Target.TimeScaleIndependent = value;
        }

        public void SetUseGUILayout(bool value) {
            if (Target == null) {
                return;
            }

            Target.useGUILayout = value;
        }

        public void SetRunInEditMode(bool value) {
            if (Target == null) {
                return;
            }

            Target.runInEditMode = value;
        }

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