using UnityEngine.Events;

namespace Datenshi.Scripts.Combat {
    public class GlobalDamageEvent : UnityEvent<DamageInfo> {
        public static readonly GlobalDamageEvent Instance = new GlobalDamageEvent();
        private GlobalDamageEvent() { }
    }
}