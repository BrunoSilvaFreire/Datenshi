using System;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.Util.Buffs;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Status {
    [Serializable, CreateAssetMenu(menuName = "Datenshi/Combat/StatusEffect/Speed")]
    public class SpeedStatusEffect : StatusEffect {
        public const float SpeedColorHue = 0.419444444F;
        public const string SpeedAlias = "Speed";
        public float Magnitude;
        public float Duration;
        public bool ClearOthers;


        protected override PropertyModifier OnApply(ICombatant combatant) {
            var movable = combatant as IMovable;
            if (movable == null) {
                return null;
            }

            var m = movable.SpeedMultiplier;
            if (ClearOthers) {
                m.ClearModifiers();
            }

            return m.AddPeriodicModifier(Duration, Magnitude);
        }


        protected override float GetHue() {
            return SpeedColorHue;
        }

        public override string GetAlias() {
            return SpeedAlias;
        }
    }
}