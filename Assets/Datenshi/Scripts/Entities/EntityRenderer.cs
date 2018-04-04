using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public class EntityRenderer : MonoBehaviour {
        public const string OverrideAmountKey = "_OverrideAmount";
        public const string OverrideColorKey = "_OverrideColor";
        public const string ShaderName = "Datenshi/EntityShader";
        public SpriteRenderer Renderer;
        public Entity Entity;
        public Color DefaultOverrideColor = Color.red;

        [ReadOnly]
        private Material material;

        [ShowInInspector]
        private void Reload() {
            material = null;
            EnsureMaterialSet();
        }

        private void OnValidate() {
            EnsureMaterialSet();
        }

        private void EnsureMaterialSet() {
            if (material != null) {
                return;
            }

            material = new Material(Shader.Find(ShaderName));
            var c = Entity.Character;
            var color = c != null ? c.SignatureColor : DefaultOverrideColor;
            material.SetColor(OverrideColorKey, color);
            Renderer.material = material;
        }

        private void Awake() {
            EnsureMaterialSet();
        }

        [ShowInInspector]
        public float ColorOverrideAmount {
            get {
                if (Renderer == null) {
                    return 0;
                }
#if UNITY_EDITOR
                if (PrefabUtility.GetPrefabParent(Renderer) == null) {
                    return 0;
                }
#endif
                var rendererMat = Renderer.material;
                return rendererMat == null ? 0 : rendererMat.GetFloat(OverrideAmountKey);
            }
            set {
                Renderer.material.SetFloat(OverrideAmountKey, value);
            }
        }
    }
}