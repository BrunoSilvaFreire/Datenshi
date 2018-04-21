using System;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Misc {
    [Serializable]
    public sealed class ColorizableRendererEvent : UnityEvent<ColorizableRenderer> { }

    [ExecuteInEditMode]
    public class ColorizableRenderer : MonoBehaviour {
        public const string OverrideAmountKey = "_OverrideAmount";
        public const string ColorKey = "_Color";
        public SpriteRenderer[] Renderers;

        private MaterialPropertyBlock block;

        public static readonly ColorizableRendererEvent
            ColorizableRendererEnabledEvent = new ColorizableRendererEvent();

        public static readonly ColorizableRendererEvent ColorizableRendererDisabledEvent =
            new ColorizableRendererEvent();

        private void Awake() {
            ColorizableRendererEnabledEvent.Invoke(this);
        }

        private void OnDisable() {
            ColorizableRendererDisabledEvent.Invoke(this);
        }

        private void Update() {
            if (block == null) {
                block = new MaterialPropertyBlock();
            }

            foreach (var r in Renderers) {
                if (r == null) {
                    continue;
                }
                r.GetPropertyBlock(block);
                block.SetFloat(OverrideAmountKey, ColorOverrideAmount);
                block.SetColor(ColorKey, r.color);
                r.SetPropertyBlock(block);
            }
        }

        public float ColorOverrideAmount;
    }
}