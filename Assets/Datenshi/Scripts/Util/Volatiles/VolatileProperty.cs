using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Util.Volatiles {
    [Serializable]
    public abstract class VolatileProperty<T> {
        private static readonly Predicate<VolatilePropertyModifier<T>> EffectorTicker = modifier => modifier.Tick();
        public T BaseValue;

        [SerializeField, ShowInInspector, ReadOnly]
        private List<VolatilePropertyModifier<T>> effectors = new List<VolatilePropertyModifier<T>>();

        public IEnumerable<VolatilePropertyModifier<T>> Effectors => effectors;

        public abstract T Value {
            get;
        }
#if UNITY_EDITOR

        [ShowInInspector]
        private void AddTestMultiplier() {
            AddModifier(2, 5);
        }
#endif
        public void Clear() {
            foreach (var mod in effectors) {
                mod.TryInvokeCallback();
            }

            effectors.Clear();
        }

        public void Tick() {
            effectors.RemoveAll(EffectorTicker);
        }

        public VolatilePropertyModifier<T> AddModifier(float magnitude, float duration, ModifierRemovedCallback callback = null) {
            var modifier = new VolatilePropertyModifier<T>(this, callback, duration, magnitude);
            effectors.Add(modifier);
            return modifier;
        }
    }
}