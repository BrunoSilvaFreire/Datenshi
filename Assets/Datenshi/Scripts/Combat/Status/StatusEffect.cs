using Datenshi.Scripts.Movement;

namespace Datenshi.Scripts.Combat.Status {
    public abstract class StatusEffect {
        public abstract void Apply(ICombatant combatant);
    }

    public class SpeedStatusEffect : StatusEffect {
        public float Magnitude {
            get;
        }

        public float Duration {
            get;
        }

        public SpeedStatusEffect(float magnitude, float duration) {
            Magnitude = magnitude;
            Duration = duration;
        }

        public override void Apply(ICombatant combatant) {
            var movable = combatant as IDatenshiMovable;
            movable?.SpeedMultiplier.AddModifier(Magnitude, Duration);
        }
    }
}