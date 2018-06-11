using System.Collections.Generic;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Status;
using Sirenix.Utilities;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity {
        private readonly List<StatusEffect> effects = new List<StatusEffect>();

        public void ApplyStatusEffect(StatusEffect e) {
            effects.Add(e);
        }

        public IEnumerable<T> FindStatusEffects<T>() where T : StatusEffect {
            return effects.FilterCast<T>();
        }


        private void UpdateStatusEffects() {
            foreach (var effect in effects) {
                effect.OnTick(this);
            }
        }
    }
}