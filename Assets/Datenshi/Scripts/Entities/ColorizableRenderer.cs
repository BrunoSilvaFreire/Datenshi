using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    [Serializable]
    public sealed class ColorizableRendererEvent : UnityEvent<ColorizableRenderer> { }

    public class ColorizableRenderer : MonoBehaviour {
        public const string OverrideAmountKey = "_OverrideAmount";
        public SpriteRenderer[] Renderers;

        private float overrideAmount;

        private MaterialPropertyBlock block;
        public static readonly ColorizableRendererEvent ColorizableRendererEnabledEvent = new ColorizableRendererEvent();
        public static readonly ColorizableRendererEvent ColorizableRendererDisabledEvent = new ColorizableRendererEvent();

        private void Awake() {
            ColorizableRendererEnabledEvent.Invoke(this);
        }

        private void OnDisable() {
            ColorizableRendererDisabledEvent.Invoke(this);
        }

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