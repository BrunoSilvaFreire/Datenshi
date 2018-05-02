using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks.Summoning {
    [CreateAssetMenu(menuName = "Datenshi/Combat/SummonAttack")]
    public class SummonAttack : Attack {
        public Summonable Prefab;

        public override void Execute(ICombatant entity) {
            var target = entity.GetVariable(CombatVariables.AttackTarget);
            var pos = target.Center;
            var proj = Prefab.Clone(pos);
            proj.Summon(entity, pos);
        }
    }
}