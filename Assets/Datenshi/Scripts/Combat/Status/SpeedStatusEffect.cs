using System;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.Util.Volatiles;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Status {
    [Serializable, CreateAssetMenu(menuName = "Datenshi/Combat/StatusEffect/Speed")]
    public class SpeedStatusEffect : StatusEffect {
        public const float SpeedColorHue = 0.419444444F;
        public const string SpeedAlias = "Speed";
        public float Magnitude;
        public float Duration;
        public bool ClearOthers;


        protected override VolatilePropertyModifier OnApply(ICombatant combatant) {
            var movable = combatant as IDatenshiMovable;
            if (movable == null) {
                return null;
            }

            var m = movable.SpeedMultiplier;
            if (ClearOthers) {
                m.Clear();
            }

            var mod = m.AddModifier(Magnitude, Duration, () => { RemoveFromEffectsList(combatant, this); });

            return mod;
        }


        protected override float GetHue() {
            return SpeedColorHue;
        }

        public override string GetAlias() {
            return SpeedAlias;
        }
    }
}