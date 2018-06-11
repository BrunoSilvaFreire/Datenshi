using Datenshi.Scripts.Movement;
using UnityEngine;
using UPM.Motors;

namespace Datenshi.Scripts.Combat.Status {
    public abstract class StatusEffect {
        public virtual void OnApply(ICombatant combatant) { }
        public virtual bool OnTick(ICombatant combatant) {
            return true;
        }
    }

    public abstract class TimeStatusEffect : StatusEffect {
        private readonly float duration;
        private float timeLeft;

        public override bool OnTick(ICombatant combatant) {
            timeLeft -= Time.deltaTime;
            return timeLeft <= 0;
        }
    }

    public class SpeedStatusEffect : TimeStatusEffect {
        private readonly float magnitude;

        public SpeedStatusEffect(float magnitude) {
            this.magnitude = magnitude;
        }

        public float Magnitude => magnitude;
    }
}