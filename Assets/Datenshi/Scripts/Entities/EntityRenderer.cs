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

        private float overrideAmount;

        private MaterialPropertyBlock block;

        [ShowInInspector]
        public float ColorOverrideAmount {
            get {
                return overrideAmount;
            }
            set {
                overrideAmount = value;
                if (block == null) {
                    block = new MaterialPropertyBlock();
                }

                foreach (var renderer in Renderers) {
                    renderer.GetPropertyBlock(block);
                    block.SetFloat(OverrideAmountKey, value);

                    renderer.SetPropertyBlock(block);
                    
                }
            }
        }
    }
}