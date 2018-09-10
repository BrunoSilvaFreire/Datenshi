using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Util.Volatiles;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity : ICombatant {
        [SerializeField]
        private CombatRelationship relationship;

        public CombatRelationship Relationship => relationship;

        [TitleGroup(HealthGroup, "Informações sobre a vida desta LivingEntity")]
        public bool DamageInvulnerability;

        [ShowIf("DamageInvulnerability")]
        public float DamageInvulnerabilityDuration = 3;

        [TitleGroup(CombatGroup)]
        public EntityAttackEvent OnAttack;

        [TitleGroup(CombatGroup)]
        public float FocusAnimationSpeed = 2;

        [SerializeField]
        private FloatVolatileProperty damageMultiplier;


        public FloatVolatileProperty DamageMultiplier => damageMultiplier;

        public bool IsEnemy(LivingEntity entity) {
            if (Relationship == CombatRelationship.Neutral || entity.Relationship == CombatRelationship.Neutral) {
                return false;
            }

            return Relationship != entity.Relationship;
        }

        public bool IsNeutral => Relationship == CombatRelationship.Neutral;

        public bool IsAlly => Relationship == CombatRelationship.Ally;

        public void ExecuteSkill(ActiveSkill skill) {
            if (Stunned || skill == null) {
                return;
            }

            var attack = skill as Attack;
            if (attack != null) {
                OnAttack.Invoke(attack);
            }

            skill.Execute(this);
        }

        [ShowInInspector, TitleGroup(CombatGroup)]
        private bool focusing;

        public bool Focusing {
            get {
                return focusing;
            }
            set {
                if (value && !canRefocus) {
                    return;
                }

                if (value && !CanFocus) {
                    return;
                }

                focusing = value;
            }
        }

        private bool canRefocus;

        [TitleGroup(CombatGroup)]
        public float FocusRecoverAmountMultiplier = 1;

        [TitleGroup(CombatGroup)]
        public float FocusDepleteAmountMultiplier = 2;

        [TitleGroup(CombatGroup)]
        public float FocusDamageMultiplier = 1.5F;

        private void UpdateFocus() {
            var p = InputProvider;
            if (p != null) {
                var pressingFocus = p.GetFocus();
                if (Focusing) {
                    if (focusingLastFrame && !CanFocus) {
                        canRefocus = false;
                        Focusing = false;
                    }
                } else {
                    if (!canRefocus) {
                        canRefocus = !pressingFocus;
                    }
                }

                Focusing = pressingFocus;
            }

            damageMultiplier.BaseValue = Focusing ? FocusDamageMultiplier : 1;
            focusingLastFrame = Focusing;
            if (Focusing) {
                FocusTimeLeft -= Time.deltaTime * FocusDepleteAmountMultiplier;
            }

            var a = AnimatorUpdater != null ? AnimatorUpdater.Animator : null;
            if (a == null) {
                return;
            }

            a.speed = focusing ? FocusAnimationSpeed : 1;
        }
    }
}