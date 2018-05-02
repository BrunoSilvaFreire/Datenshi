using System;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Graphics {
    [Serializable]
    public sealed class ColorizableRendererEvent : UnityEvent<ColorizableRenderer> { }

    public interface IColorizable {
        ColorizableRenderer ColorizableRenderer {
            get;
        }
    }

    [ExecuteInEditMode]
    public class ColorizableRenderer : MonoBehaviour {
        public const string OverrideAmountKey = "_OverrideAmount";
        public const string ColorKey = "_Color";
        public Renderer[] Renderers;

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
                Color color;
                var spriteRenderer = r as SpriteRenderer;
                if (spriteRenderer != null) {
                    color = spriteRenderer.color;
                } else if (r is LineRenderer) {
                    color = ((LineRenderer) r).endColor;
                } else if (r is TrailRenderer) {
                    color = ((TrailRenderer) r).endColor;
                } else {
                    continue;
                }

                block.SetFloat(OverrideAmountKey, ColorOverrideAmount);
                block.SetColor(ColorKey, color);
                r.SetPropertyBlock(block);
            }
        }

        public float ColorOverrideAmount;
    }
}