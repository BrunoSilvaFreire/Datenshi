using System.Collections;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks.UI {
    [CreateAssetMenu(menuName = "Datenshi/Combat/RepeatedHitboxAttack")]
    public class RepeatedHitboxAttack : AbstractHitboxAttack {
        public uint TotalAttacks;
        public uint DamagePerAttack;
        public float AttackDelay = 0.01666667F;

        public override void Execute(ICombatant entity) {
            entity.AnimatorUpdater.StartCoroutine(DoRepeatedAttack(entity));
        }

        private IEnumerator DoRepeatedAttack(ICombatant entity) {
            for (uint i = 0; i < TotalAttacks; i++) {
                DoAttack(entity, DamagePerAttack);
                yield return new WaitForSeconds(AttackDelay);
            }
        }
    }
}