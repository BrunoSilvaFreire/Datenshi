using Datenshi.Scripts.Graphics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class MaterialPropertyBlockViewer : MonoBehaviour {
        [ShowInInspector, ReadOnly]
        private new Renderer renderer;

        [ShowInInspector, ReadOnly]
        private MaterialPropertyBlock block;

        [ShowInInspector]
        public Color Color => block == null ? Color.magenta : block.GetColor("_Color");

        [ShowInInspector]
        public Texture Texture => block == null ? null : block.GetTexture("_MainTex");

        public float OverrideAmount => block == null ? -1 : block.GetFloat(ColorizableRenderer.OverrideAmountKey);

        private void OnValidate() {
            if (block == null) {
                block = new MaterialPropertyBlock();
            }

            if (renderer == null) {
                renderer = GetComponent<Renderer>();
            }

            if (renderer != null) {
                renderer.GetPropertyBlock(block);
            }
        }
    }
}