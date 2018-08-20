using UnityEngine;

namespace Datenshi.Scripts.Graphics {
    public class ColorizableTrailSlave : AbstractSlave<TrailRenderer> {
        public Gradient Gradient;

        public override void Initialize(TrailRenderer renderer) {
            base.Initialize(renderer);
            Gradient = renderer.colorGradient;
        }
    }
}