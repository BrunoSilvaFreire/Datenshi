using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Graphics {
    public abstract class StandaloneVisualFX : VisualFX {
        [SerializeField, ReadOnly]
        protected Material Material;

        public override Material GetMaterial() {
            return Material ? Material : (Material = LoadMaterial());
        }

        private Material LoadMaterial() {
            var shaderName = GetShaderName();
            var shader = Shader.Find(shaderName);
            if (shader != null) {
                return new Material(shader);
            }

            Debug.LogWarningFormat("Couldn't find shader '{0}' for black and white effect", shaderName);
            return null;
        }

        protected abstract string GetShaderName();
    }

    public abstract class VisualFX : MonoBehaviour {
        public abstract Material GetMaterial();

        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            var m = GetMaterial();
            if (m == null) {
                return;
            }
            UnityEngine.Graphics.Blit(source, destination, m);
        }
    }
}