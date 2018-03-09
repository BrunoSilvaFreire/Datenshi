using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    [CreateAssetMenu(menuName = "Origame/Combat/HitboxAttack")]
    public class HitboxAttack : AbstractHitboxAttack {
        public uint Damage;

        public override void Execute(LivingEntity entity) {
            DoAttack(entity, Damage);
        }
    }
}