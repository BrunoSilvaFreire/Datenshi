using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Assets.Graphics.Shaders {
    public class OverrideColorFX : MonoBehaviour {
        private const string ShaderName = "Datenshi/OverrideColorShader";
        private const string PropertyName = "_Amount";
        private const string ColorPropertyName = "_Color";

        //[ShowInInspector, ReadOnly]
        public Material material;

        [ShowInInspector]
        public float Amount {
            get {
                EnsureMaterial();
                return material.GetFloat(PropertyName);
            }
            set {
                EnsureMaterial();
                material.SetFloat(PropertyName, value);
            }
        }


        private void OnDestroy() {
            if (material == null) {
                return;
            }

            DestroyImmediate(material);
        }

        [ShowInInspector]
        public Color Color {
            get {
                EnsureMaterial();
                return material.GetColor(ColorPropertyName);
            }
            set {
                EnsureMaterial();
                material.SetColor(ColorPropertyName, value);
            }
        }

        private void EnsureMaterial() {
            if (material != null) {
                return;
            }

            var shader = Shader.Find(ShaderName);
            material = new Material(shader) {
                hideFlags = HideFlags.DontSave
            };
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            EnsureMaterial();
            UnityEngine.Graphics.Blit(source, destination, material);
        }
    }
}