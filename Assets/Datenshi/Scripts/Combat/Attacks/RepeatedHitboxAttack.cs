using System.Collections;
using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    [CreateAssetMenu(menuName = "Datenshi/Combat/RepeatedHitboxAttack")]
    public class RepeatedHitboxAttack : AbstractHitboxAttack {
        public uint TotalAttacks;
        public uint DamagePerAttack;
        public float AttackDelay = 0.01666667F;

        public override void Execute(LivingEntity entity) {
            entity.StartCoroutine(DoRepeatedAttack(entity));
        }

        private IEnumerator DoRepeatedAttack(LivingEntity entity) {
            for (uint i = 0; i < TotalAttacks; i++) {
                DoAttack(entity, DamagePerAttack);
                yield return new WaitForSeconds(AttackDelay);
            }
        }
    }
}