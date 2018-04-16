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
                return material != null ? material.GetFloat(PropertyName) : 0;
            }
            set {
                EnsureMaterial();
                if (material != null) {
                    material.SetFloat(PropertyName, value);
                }
            }
        }

        [ShowInInspector]
        public float DarkenAmount {
            get {
                EnsureMaterial();
                return material != null ? material.GetFloat(DarkenPropertyName) : 0;
            }
            set {
                EnsureMaterial();
                if (material != null) {
                    material.SetFloat(DarkenPropertyName, value);
                }
            }
        }

        private void EnsureMaterial() {
            if (material != null) {
                return;
            }

            var shader = Shader.Find(ShaderName);
            if (shader == null) {
                Debug.LogWarningFormat("Couldn't find shader '{0}' for black and white effect", ShaderName);
                return;
            }

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
            if (material != null) {
                UnityEngine.Graphics.Blit(source, destination, material);
            }
        }
    }
}