using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Assets.Graphics.Shaders {
    [ExecuteInEditMode]
    public class BlackAndWhiteFX : MonoBehaviour {
        private const string ShaderName = "Datenshi/BlackAndWhiteShader";
        private const string DarkenPropertyName = "_DarkenAmount";
        private const string PropertyName = "_Amount";

        [ShowInInspector, ReadOnly]
        private Material material;

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

        [ShowInInspector]
        public float DarkenAmount {
            get {
                EnsureMaterial();
                return material.GetFloat(DarkenPropertyName);
            }
            set {
                EnsureMaterial();
                material.SetFloat(DarkenPropertyName, value);
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

        private void OnDestroy() {
            if (material == null) {
                return;
            }

            DestroyImmediate(material);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            EnsureMaterial();
            UnityEngine.Graphics.Blit(source, destination, material);
        }
    }
}