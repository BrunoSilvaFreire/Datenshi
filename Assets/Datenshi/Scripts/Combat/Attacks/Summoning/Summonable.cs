using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Misc;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks.Summoning {
    public class Summonable : Ownable {
        protected void ExecuteProxiedAttack(Attack attack) {
            if (Owner == null) {
                return;
            }

            Owner.ExecuteAttack(attack);
        }

        public void Summon(LivingEntity entity, Vector2 position) {
            Owner = entity;
            transform.position = position;
        }
    }
}