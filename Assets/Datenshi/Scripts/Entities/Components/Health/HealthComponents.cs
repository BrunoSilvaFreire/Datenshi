using System.Collections.Generic;
using Datenshi.Scripts.Health;
using Entitas;

namespace Datenshi.Scripts.Entities.Components.Health {
    [Game]
    public class HealthComponent : IComponent {
        public int Health = 200;
        public int MaxHealth = 200;
    }

    [Game]
    public class DamageHistoryComponent : IComponent {
        public List<DamageCause> LatestCauses;
    }
}