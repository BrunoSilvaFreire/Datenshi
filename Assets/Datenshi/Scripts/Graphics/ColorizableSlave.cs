using UnityEngine;

namespace Datenshi.Scripts.Graphics {
    public class ColorizableSlave : AbstractSlave<SpriteRenderer> {
        public Color SpriteColor;

        public override void Initialize(SpriteRenderer renderer) {
            base.Initialize(renderer);
            SpriteColor = renderer.color;
        }
    }
}