using Datenshi.Scripts.Data;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks.Summoning {

    public class Summonable : Ownable<ICombatant> {
        protected void ExecuteProxiedAttack(Attack attack) {
            if ((Object) Owner == null) {
                return;
            }

            Owner.ExecuteSkill(attack);
        }

        public void Summon(ICombatant entity, Vector2 position) {
            Owner = entity;
            transform.position = position;
            //entity.AnimatorUpdater.TriggerSpawn();
        }
    }
}