using System.Collections.Generic;
using Shiroi.FX.Services;
using UnityEngine;

namespace Datenshi.Scripts.Graphics.Colorization {


    public class OutlineController : AbstractColorController<ColorMeta> {
        public const string OutlineColorKey = "_OutlineColor";

        protected override void ApplyToBlock(MaterialPropertyBlock block, ColorMeta meta) {
            block.SetColor(OutlineColorKey, meta.Color);
        }

        protected override ColorMeta GetEmptyMeta() {
            return new ColorMeta(Color.clear, false);
        }

        protected override void Accumulate(ref ColorMeta dest, ColorMeta source, float weight) {
            dest.Color += source.Color * weight;
        }
    }
}