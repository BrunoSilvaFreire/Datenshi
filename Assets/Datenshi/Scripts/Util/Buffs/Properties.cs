using System;
using System.Collections.Generic;
using System.Linq;
using Lunari.Tsuki;

namespace Datenshi.Scripts.Util.Buffs {
    public abstract class Property { }

    /// <summary>
    /// Representa uma propriedade que pode ser modificada temporáriamente (Buffs e Debuffs), mantendo um valor base.
    /// <b>NÃO USE ESSA CLASSE DIRETAMENTE.</b> Use suas implementações como por exemplo: <see cref="FloatProperty"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// 
    public abstract class Property<T> : Property {
        private List<PropertyModifier> modifiers;


        public static implicit operator T(Property<T> prop) {
            return prop.Value;
        }

        protected Property(T baseValue) {
            BaseValue = baseValue;
        }

        public T BaseValue;

        public IList<PropertyModifier> Modifiers => modifiers ?? (modifiers = new List<PropertyModifier>());

        public T Value {
            get {
                if (modifiers.IsNullOrEmpty()) {
                    return BaseValue;
                }

                var finalMultiplier =
                    (from modifier in Modifiers select modifier.Multiplier).Aggregate((a, b) => a * b);
                return Multiply(BaseValue, finalMultiplier);
            }
        }

        protected abstract T Multiply(T baseValue, float aggregateMultipliers);


        public void AddModifier(PropertyModifier modifier) {
            Modifiers.Add(modifier);
        }

        // TODO: Dar um jeito de esse método não precisar ser explicitamente chamado?
        // Usar MonoBehaviour ou os modificadores devem rodar como uma corotina?
        public void Tick() {
            if (modifiers.IsNullOrEmpty()) {
                return;
            }

            modifiers.RemoveAll(modifier => modifier.Tick());
        }
    }

    [Serializable]
    public sealed class FloatProperty : Property<float> {
        public FloatProperty(float baseValue = 1) : base(baseValue) { }

        public static implicit operator FloatProperty(float value) {
            return new FloatProperty(value);
        }

        protected override float Multiply(float baseValue, float aggregateMultipliers) {
            return baseValue * aggregateMultipliers;
        }

        public void ClearModifiers() {
            foreach (var propertyModifier in Modifiers) {
                propertyModifier.Cancel();
            }
        }
    }

    [Serializable]
    public sealed class UInt32Property : Property<uint> {
        public UInt32Property(uint baseValue) : base(baseValue) { }

        protected override uint Multiply(uint baseValue, float aggregateMultipliers) {
            return (uint) (baseValue * aggregateMultipliers);
        }

        public static implicit operator UInt32Property(uint value) {
            return new UInt32Property(value);
        }
    }

    [Serializable]
    public sealed class Int32Property : Property<int> {
        public Int32Property(int baseValue) : base(baseValue) { }

        protected override int Multiply(int baseValue, float aggregateMultipliers) {
            return (int) (baseValue * aggregateMultipliers);
        }

        public static implicit operator Int32Property(int value) {
            return new Int32Property(value);
        }
    }

    public static class PropertyExtensions {
        public static ContinuousPropertyModifier AddContinuousModifiers<T>(this Property<T> property, float value) {
            var modifier = new ContinuousPropertyModifier(value);
            property.AddModifier(modifier);
            return modifier;
        }

        public static PeriodicPropertyModifier AddPeriodicModifier
            <T>(this Property<T> property, float duration, float value) {
            var modifier = new PeriodicPropertyModifier(duration, value);
            property.AddModifier(modifier);
            return modifier;
        }
    }
}