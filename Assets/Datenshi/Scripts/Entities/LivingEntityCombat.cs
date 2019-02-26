using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Util.Buffs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity : ICombatant {
        public const string CombatGroup = "Combat";

        [SerializeField, BoxGroup(CombatGroup)]
        private CombatRelationship relationship;

        public CombatRelationship Relationship => relationship;

        [BoxGroup(CombatGroup)]
        public bool DamageInvulnerability;

        [ShowIf(nameof(DamageInvulnerability)), BoxGroup(CombatGroup)]
        public float DamageInvulnerabilityDuration = 3;

        [BoxGroup(CombatGroup)]
        public EntityAttackEvent OnAttack;

        [SerializeField, BoxGroup(CombatGroup)]
        private FloatProperty damageMultiplier;


        public FloatProperty DamageMultiplier => damageMultiplier;


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
    }
}