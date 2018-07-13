using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    public abstract class ActiveSkill : ScriptableObject {
        public abstract void Execute(ICombatant entity);
    }

    public abstract class Attack : ActiveSkill, IDamageSource {
        public abstract uint GetDamage(IDamageable damageable);
    }
}