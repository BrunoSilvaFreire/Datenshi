using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    public abstract class Attack : ScriptableObject {
        public abstract void Execute(ICombatant entity);

        public abstract uint GetDamage(ICombatant livingEntity);
    }
}