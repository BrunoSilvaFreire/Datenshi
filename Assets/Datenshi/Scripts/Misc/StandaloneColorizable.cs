using UnityEngine;

namespace Datenshi.Scripts.Misc {
    [ExecuteInEditMode]
    public class StandaloneColorizable : MonoBehaviour {
        public const string OverrideAmountKey = "_OverrideAmount";
        public const string ColorKey = "_Color";
        public SpriteRenderer[] Renderers;
        public Light[] Lights;
        public ParticleSystem[] Particles;
        public float ColorOverrideAmount;
        public Color Color;
        private MaterialPropertyBlock block;

        private void Update() {
            if (block == null) {
                block = new MaterialPropertyBlock();
            }

            foreach (var r in Renderers) {
                if (r == null) {
                    continue;
                }

                r.GetPropertyBlock(block);
                block.SetFloat(OverrideAmountKey, ColorOverrideAmount);
                block.SetColor(ColorKey, Color);
                r.SetPropertyBlock(block);
            }

            foreach (var particle in Particles) {
                var main = particle.main;
                main.startColor = Color;
            }

            foreach (var light in Lights) {
                light.color = Color;
            }
        }
    }
}