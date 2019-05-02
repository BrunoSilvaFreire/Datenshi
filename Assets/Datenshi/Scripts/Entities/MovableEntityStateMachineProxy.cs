using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    public class MovableEntityStateMachineProxy : StateMachineBehaviour {
        [ShowInInspector, ReadOnly]
        private MovableEntity target;

        public UnityEvent OnEnter, OnExit;


        private void TestConfigured(Component animator) {
            if (target != null) {
                return;
            }

            target = animator.GetComponentInParent<MovableEntity>();
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            TestConfigured(animator);
            OnEnter.Invoke();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            TestConfigured(animator);
            OnExit.Invoke();
        }

        public void ExecuteSkill(ActiveSkill skill) {
            target.ExecuteSkill(skill);
        }

        public void BreakDefense() {
            target.BreakDefense();
        }

        public void SetInvulnerable(float seconds) {
            target.SetInvulnerable(seconds);
        }

        public void Kill() {
            target.Kill();
        }

        public void Heal(uint healthAmount) {
            target.Heal(healthAmount);
        }

        public void Heal() {
            target.Heal();
        }

        public void Stun(float duration) {
            target.Stun(duration);
        }

        public void RevokeOwnership() {
            target.RevokeOwnership();
        }

        public void ForceRequestOwnership(DatenshiInputProvider player) {
            target.ForceRequestOwnership(player);
        }

        public void SetRigidbody(Rigidbody2D value) {
            if (target == null) {
                return;
            }

            target.Rigidbody = value;
        }

        public void SetRigidStateHolder(GameObject value) {
            if (target == null) {
                return;
            }

            target.RigidStateHolder = value;
        }


        public void SetSpeedMultiplier(float value) {
            if (target == null) {
                return;
            }

            target.SpeedMultiplier.BaseValue = value;
        }

        public void SetDirectionChangeThreshold(float value) {
            if (target == null) {
                return;
            }

            target.DirectionChangeThreshold = value;
        }

        public void SetOutlineInvulnerabilityMinSecondsLeft(float value) {
            if (target == null) {
                return;
            }

            target.OutlineInvulnerabilityMinSecondsLeft = value;
        }

        public void SetDamageInvulnerability(bool value) {
            if (target == null) {
                return;
            }

            target.DamageInvulnerability = value;
        }

        public void SetDamageInvulnerabilityDuration(float value) {
            if (target == null) {
                return;
            }

            target.DamageInvulnerabilityDuration = value;
        }

        public void SetOnAttack(EntityAttackEvent value) {
            if (target == null) {
                return;
            }

            target.OnAttack = value;
        }

        public void SetMinFocusRequired(float value) {
            if (target == null) {
                return;
            }

            target.MinDefenseRequired = value;
        }

        public void SetOnHealthChanged(UnityEvent value) {
            if (target == null) {
                return;
            }

            target.OnHealthChanged = value;
        }

        public void SetDamageColor(Color value) {
            if (target == null) {
                return;
            }

            target.DamageColor = value;
        }

        public void SetDamageColorAmount(float value) {
            if (target == null) {
                return;
            }

            target.DamageColorAmount = value;
        }

        public void SetDefenseBreakStunDuration(float value) {
            if (target == null) {
                return;
            }

            target.DefenseBreakStunDuration = value;
        }

        public void SetDamageGivesStun(bool value) {
            if (target == null) {
                return;
            }

            target.DamageGivesStun = value;
        }

        public void SetDamageStunMin(uint value) {
            if (target == null) {
                return;
            }

            target.DamageStunMin = value;
        }

        public void SetDamageStunDuration(float value) {
            if (target == null) {
                return;
            }

            target.DamageStunDuration = value;
        }

        public void SetMiscController(EntityMiscController value) {
            if (target == null) {
                return;
            }

            target.MiscController = value;
        }

        public void SetCharacter(Character.Character value) {
            if (target == null) {
                return;
            }

            target.Character = value;
        }

        public void SetCurrentDirection(Direction value) {
            if (target == null) {
                return;
            }

            target.CurrentDirection = value;
        }

        public void SetIgnored(bool value) {
            if (target == null) {
                return;
            }

            target.Ignored = value;
        }

        public void SetDefending(bool value) {
            if (target == null) {
                return;
            }

            target.Defending = value;
        }

        public void SetHealthPercentage(float value) {
            if (target == null) {
                return;
            }

            target.HealthPercentage = value;
        }

        public void SetGodMode(bool value) {
            if (target == null) {
                return;
            }

            target.GodMode = value;
        }

        public void SetMaxHealth(uint value) {
            if (target == null) {
                return;
            }

            target.MaxHealth = value;
        }

        public void SetTimeScaleIndependent(bool value) {
            if (target == null) {
                return;
            }

            target.TimeScaleIndependent = value;
        }

        public void SetUseGUILayout(bool value) {
            if (target == null) {
                return;
            }

            target.useGUILayout = value;
        }

        public void SetRunInEditMode(bool value) {
            if (target == null) {
                return;
            }

            target.runInEditMode = value;
        }

        public void SetEnabled(bool value) {
            if (target == null) {
                return;
            }

            target.enabled = value;
        }

        public void SetTag(string value) {
            if (target == null) {
                return;
            }

            target.tag = value;
        }

        public void SetName(string value) {
            if (target == null) {
                return;
            }

            target.name = value;
        }

        public void SetHideFlags(HideFlags value) {
            if (target == null) {
                return;
            }

            target.hideFlags = value;
        }
    }
}