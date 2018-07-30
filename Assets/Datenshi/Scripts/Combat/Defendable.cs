using System;
using UnityEngine.Events;

namespace Datenshi.Scripts.Combat {
    [Serializable]
    public class GlobalDefenseEvent : UnityEvent<ICombatant, DamageInfo> {
        public static readonly GlobalDefenseEvent Instance = new GlobalDefenseEvent();
        private GlobalDefenseEvent() { }
    }

    public interface IDefendable {
        bool CanDefend(ICombatant combatant);
        float Defend(ICombatant combatant, ref DamageInfo info);
    }
}