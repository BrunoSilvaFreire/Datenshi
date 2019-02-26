using UnityEngine;

namespace Datenshi.Scripts.Util.Buffs {
    /// <summary>
    /// Representa um único modificador de uma <see cref="Property"/>.
    /// </summary>
    public abstract class PropertyModifier {
        /// <summary>
        /// Cancela o uso deste modificador pela propriedade em que ele está sendo usado.
        /// <br/>
        /// Após esse método ser chamado, o próximo <see cref="Tick"/> <b>DEVE</b> obrigatóriamente retornar false.
        /// </summary>
        public abstract void Cancel();

        /// <summary>
        /// Atualiza o modificador, chamado uma vez por frame.
        /// </summary>
        /// <returns>true se o modificador expirou ou foi cancelado e deve ser retirado da lista de modificadores.</returns>
        public abstract bool Tick();

        public abstract float Multiplier {
            get;
            set;
        }
    }

    public class ContinuousPropertyModifier : PropertyModifier {
        private bool isCancelled;

        public ContinuousPropertyModifier(float multiplier) {
            isCancelled = false;
            Multiplier = multiplier;
        }

        public override void Cancel() {
            isCancelled = true;
        }

        public override bool Tick() {
            return isCancelled;
        }

        public override float Multiplier {
            get;
            set;
        }
    }

    public class PeriodicPropertyModifier : PropertyModifier {
        public override void Cancel() {
            TimeLeft = 0;
        }

        public override bool Tick() {
            return (TimeLeft -= Time.deltaTime) <= 0;
        }

        public float TimeLeft {
            get;
            private set;
        }

        public float Duration {
            get;
        }

        public float PercentCompleted => TimeLeft / Duration;

        public PeriodicPropertyModifier(float duration, float multiplier) {
            Duration = duration;
            TimeLeft = duration;
            Multiplier = multiplier;
        }

        public sealed override float Multiplier {
            get;
            set;
        }
    }
}