using System;
using System.Collections.Generic;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Buffs;
using Lunari.Tsuki;
using UnityEngine;
using UnityEngine.Events;


namespace Datenshi.Scripts.Combat.Status {
    [Serializable]
    public class StatusEffectAppliedEvent : UnityEvent<StatusEffect, ICombatant, PropertyModifier> {
        public static readonly StatusEffectAppliedEvent Instance = new StatusEffectAppliedEvent();
        private StatusEffectAppliedEvent() { }
    }

    [Serializable]
    public class StatusEffectRemovedEvent : UnityEvent<StatusEffect, ICombatant> {
        public static readonly StatusEffectRemovedEvent Instance = new StatusEffectRemovedEvent();
        private StatusEffectRemovedEvent() { }
    }

    public abstract class StatusEffect : ScriptableObject {
        private static readonly Dictionary<ICombatant, List<Tuple<StatusEffect, PropertyModifier>>> AppliedEffects
            = new Dictionary<ICombatant, List<Tuple<StatusEffect, PropertyModifier>>>();

        private static readonly Func<List<Tuple<StatusEffect, PropertyModifier>>> ListInstantiator =
            () => new List<Tuple<StatusEffect, PropertyModifier>>();

        protected static void RemoveFromEffectsList(ICombatant combatant, StatusEffect speedStatusEffect) {
            GetEffects(combatant).RemoveAll(tuple => tuple.Item1 == speedStatusEffect);
            StatusEffectRemovedEvent.Instance.Invoke(speedStatusEffect, combatant);
        }

        public static List<Tuple<StatusEffect, PropertyModifier>> GetEffects(ICombatant combatant) {
            return AppliedEffects.GetOrPut(combatant, ListInstantiator);
        }


        public const float ColorSaturation = 0.2980392F;
        public const float ColorActiveBrightness = 0.9F;
        public const float ColorInactiveBrightness = 0.1568628F;

        public void Apply(ICombatant combatant) {
            var m = OnApply(combatant);
            StatusEffectAppliedEvent.Instance.Invoke(this, combatant, m);
            GetEffects(combatant).Add(new Tuple<StatusEffect, PropertyModifier>(this, m));
        }

        protected abstract PropertyModifier OnApply(ICombatant combatant);
        protected abstract float GetHue();

        public Color GetColor() {
            return Color.HSVToRGB(GetHue(), ColorSaturation, ColorActiveBrightness);
        }

        public abstract string GetAlias();
    }
}