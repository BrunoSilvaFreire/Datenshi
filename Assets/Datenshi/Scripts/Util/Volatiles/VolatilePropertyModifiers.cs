using System;
using System.Linq;

namespace Datenshi.Scripts.Util.Volatiles {

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