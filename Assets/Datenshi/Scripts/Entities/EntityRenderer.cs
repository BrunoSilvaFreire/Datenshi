using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public class EntityRenderer : MonoBehaviour {
        public const string OverrideAmountKey = "_OverrideAmount";
        public const string OverrideColorKey = "_OverrideColor";
        public const string ShaderName = "Datenshi/EntityShader";
        public SpriteRenderer[] Renderers;
        public Entity Entity;
        public Color DefaultOverrideColor = Color.red;

        [ReadOnly, ShowInInspector]
        private Material material;

        [ShowInInspector]
        private void Reload() {
            material = null;
            EnsureMaterialSet();
        }

        private void Start() {
            Reload();
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
            foreach (var r in Renderers) {
                r.material = material;
            }
        }

        private void Awake() {
            EnsureMaterialSet();
        }

        [ShowInInspector]
        public float ColorOverrideAmount {
            get {
                EnsureMaterialSet();
                return material.GetFloat(OverrideAmountKey);
            }
            set {
                EnsureMaterialSet();
                material.SetFloat(OverrideAmountKey, value);
            }
        }
        public float 
    }
}