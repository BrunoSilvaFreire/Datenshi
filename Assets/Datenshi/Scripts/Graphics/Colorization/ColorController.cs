using UnityEngine;

namespace Datenshi.Scripts.Graphics.Colorization {
    public class ColorOverrideMeta : ColorMeta {
        private float amount;

        public ColorOverrideMeta(float amount, Color color, bool fadeAlphaOnTimedServices = true) : base(color,
            fadeAlphaOnTimedServices) {
            this.amount = amount;
        }

        public float Amount {
            get => amount;
            set => amount = value;
        }
    }

    public class ColorController : AbstractColorController<ColorOverrideMeta> {
        public const string OverrideAmountKey = "_OverrideAmount";
        public const string OverrideColorKey = "_OverrideColor";
        
        protected override void ApplyToBlock(MaterialPropertyBlock block, ColorOverrideMeta meta) {
            block.SetColor(OverrideColorKey, meta.Color);
            block.SetFloat(OverrideAmountKey, meta.Amount);
        }

        protected override ColorOverrideMeta GetEmptyMeta() {
            return new ColorOverrideMeta(0, Color.clear, false);
        }

        protected override void Accumulate(ref ColorOverrideMeta dest, ColorOverrideMeta source, float weight) {
            dest.Amount += source.Amount * weight;
            dest.Color += source.Color * weight;
        }
    }
}