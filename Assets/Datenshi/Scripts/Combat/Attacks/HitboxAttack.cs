
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    [CreateAssetMenu(menuName = "Datenshi/Combat/HitboxAttack")]
    public class HitboxAttack : AbstractHitboxAttack {
        public uint Damage;

        public override void Execute(ICombatant entity) {
            DoAttack(entity, Damage);
        }
    }
}