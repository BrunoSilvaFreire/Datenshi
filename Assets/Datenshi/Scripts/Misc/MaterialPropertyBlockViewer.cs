using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class MaterialPropertyBlockViewer : MonoBehaviour {
        [ShowInInspector, ReadOnly]
        private new Renderer renderer;

        [ShowInInspector, ReadOnly]
        private MaterialPropertyBlock block;

        [ShowInInspector]
        public Color Color => block?.GetColor("_Color") ?? Color.magenta;

        [ShowInInspector]
        public Texture Texture => block?.GetTexture("_MainTex");

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