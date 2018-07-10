using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks.Summoning {
    [CreateAssetMenu(menuName = "Datenshi/Combat/SummonAttack")]
    public class SummonAttack : Attack {
        public Summonable Prefab;
        public byte Amount;
        public override void Execute(ICombatant entity) {
            //for (var i = 0; i < Amount; i++) {
                var target = entity.GetVariable(CombatVariables.AttackTarget);
                var pos = (Object) target == null ? entity.Center : target.Center;
                var proj = Prefab.Clone(pos);
                proj.Summon(entity, pos);
            //}
        }

        public override uint GetDamage() {
            return 0;
        }
    }
}