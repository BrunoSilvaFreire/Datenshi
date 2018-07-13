using System.Collections;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    [CreateAssetMenu(menuName = "Datenshi/Combat/HitboxAttack")]
    public class HitboxAttack : AbstractHitboxAttack {
        public uint Damage;
        public byte Frames = 5;

        public override void Execute(ICombatant entity) {
            entity.AnimatorUpdater.StartCoroutine(ExecuteAttack(entity));
        }

        public override uint GetDamage(IDamageable damageable) {
            return Damage;
        }

        private IEnumerator ExecuteAttack(ICombatant entity) {
            for (byte i = 0; i < Frames; i++) {
                if (DoAttack(entity, Damage)) {
                    yield break;
                }

                yield return null;
            }
        }
    }
}