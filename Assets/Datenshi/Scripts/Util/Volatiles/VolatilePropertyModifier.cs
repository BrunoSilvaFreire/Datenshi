using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Util.Volatiles {
    public delegate void ModifierRemovedCallback();

    public class VolatilePropertyModifier {
        [ShowInInspector, HideIf(nameof(IsInfinite))]
        private float duration;

        [ShowInInspector, HideIf(nameof(IsInfinite))]
        private float timeLeft;

        private ModifierRemovedCallback callback;

        public VolatilePropertyModifier(ModifierRemovedCallback callback, float duration, float multiplier) {
            this.callback = callback;
            this.duration = duration;
            timeLeft = duration;
            Multiplier = multiplier;
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

        /// <summary>
        /// Atualiza esse modifier
        /// </summary>
        /// <returns>true se o tempo de execução desse modifier esgotou e ele deve ser retirado</returns>
        public bool Tick() {
            if (IsInfinite) {
                return false;
            }

            var r = (timeLeft -= UnityEngine.Time.deltaTime) <= 0;
            if (r) {
                TryInvokeCallback();
            }
            return r;
        }

        public void TryInvokeCallback() {
            callback?.Invoke();
        }

        public float Duration {
            get {
                return IsInfinite ? float.PositiveInfinity : duration;
            }
            set {
                duration = value;
            }
        }

        public float TimePercentage {
            get {
                if (IsInfinite) {
                    return 1;
                }

                return timeLeft / duration;
            }
        }
    }

    public class VolatilePropertyModifier<T> : VolatilePropertyModifier {
        private VolatileProperty<T> property;


        public VolatilePropertyModifier(VolatileProperty<T> property, 
            ModifierRemovedCallback callback,
            float duration, 
            float multiplier
        ) : base(callback, duration, multiplier) {
            this.property = property;
        }
    }
}