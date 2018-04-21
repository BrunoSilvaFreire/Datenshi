using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    public class AttackProxy : MonoBehaviour {
        public LivingEntity Caster;

        public void ExecuteAttack(Attack attack) {
            attack.Execute(Caster);
        }
    }
}