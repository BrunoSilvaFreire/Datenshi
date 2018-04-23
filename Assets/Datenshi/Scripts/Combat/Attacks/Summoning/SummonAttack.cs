using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks.Summoning {
    [CreateAssetMenu(menuName = "Datenshi/Combat/SummonAttack")]
    public class SummonAttack : Attack {
        public Summonable Prefab;

        public override void Execute(LivingEntity entity) {
            var target = entity.GetVariable(AttackingState.EntityTarget);
            var pos = target.transform.position;
            var proj = Prefab.Clone(pos);
            proj.Summon(entity, pos);
        }
    }
}