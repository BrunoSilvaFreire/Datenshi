using UnityEngine;

namespace Datenshi.Scripts.Graphics {
    public class ColorizableSlave : MonoBehaviour {
        public Color SpriteColor;
        public SpriteRenderer Renderer;

        public void Initialize(SpriteRenderer renderer) {
            Renderer = renderer;
            SpriteColor = Renderer.color;
        }
    }
}