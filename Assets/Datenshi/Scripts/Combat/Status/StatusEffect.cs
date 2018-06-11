using Datenshi.Scripts.Movement;
using UPM.Motors;

namespace Datenshi.Scripts.Combat.Status {
    public abstract class StatusEffect {
        public abstract void Apply(ICombatant combatant);
    }

    public class SpeedStatusEffect : StatusEffect {
        private readonly float magnitude;
        private readonly float duration;

        public SpeedStatusEffect(float magnitude, float duration) {
            this.magnitude = magnitude;
            this.duration = duration;
        }

        public override void Apply(ICombatant combatant) {
            var m = combatant as IDatenshiMovable;
            m?.AddSpeedEffector(magnitude, duration);
        }
    }
}