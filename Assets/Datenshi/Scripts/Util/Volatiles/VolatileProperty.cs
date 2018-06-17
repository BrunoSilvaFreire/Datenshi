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

        public void Tick() {
            effectors.RemoveAll(EffectorTicker);
        }

        public VolatilePropertyModifier<T> AddModifier(float magnitude, float duration) {
            var modifier = new VolatilePropertyModifier<T>(this, duration, magnitude);
            effectors.Add(modifier);
            return modifier;
        }
    }

    public class VolatilePropertyModifier<T> {
        private VolatileProperty<T> property;

        [ShowInInspector, HideIf(nameof(IsInfinite))]
        private float duration;

        [ShowInInspector, HideIf(nameof(IsInfinite))]
        private float timeLeft;

        public VolatilePropertyModifier(VolatileProperty<T> property, float duration, float multiplier) {
            this.property = property;
            this.duration = duration;
            Multiplier = multiplier;
        }

        /// <summary>
        /// Atualiza esse modifier
        /// </summary>
        /// <returns>true se o tempo de execução desse modifier esgotou e ele deve ser retirado</returns>
        public bool Tick() {
            if (IsInfinite) {
                return false;
            }

            return (timeLeft -= Time.deltaTime) > 0;
        }

        public float Duration {
            get {
                return IsInfinite ? float.PositiveInfinity : duration;
            }
            set {
                duration = value;
            }
        }

        [ShowInInspector]
        public bool IsInfinite {
            get;
        }

        [ShowInInspector]
        public float Multiplier {
            get;
            set;
        }
    }

    public class UIntVolatileProperty : VolatileProperty<uint> {
        public override uint Value {
            get {
                return Effectors.Aggregate(BaseValue, (current, modifier) => (uint) (current * modifier.Multiplier));                
            }
        }
    }

    [Serializable]
    public class FloatVolatileProperty : VolatileProperty<float> {
        public override float Value {
            get {
                return Effectors.Aggregate(BaseValue, (current, modifier) => current * modifier.Multiplier);
            }
        }
    }
}