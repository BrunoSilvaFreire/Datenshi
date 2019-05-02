using System;
using System.Collections.Generic;
using DG.Tweening;
using Shiroi.FX.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Graphics.Colorization {
    [Serializable]
    public sealed class ColorizableRendererEvent : UnityEvent<ColorizableRenderer> { }

    public interface IColorizable {
        ColorizableRenderer ColorizableRenderer { get; }
    }


    [ExecuteInEditMode]
    public class ColorizableRenderer : MonoBehaviour {
        public OutlineController OutlineController;
        public ColorController ColorController;
        private bool flipX;
        private SpriteRenderer[] renderers;
        public bool FlipX {
            get { return flipX; }
            set {
                flipX = value;
                foreach (var spriteRenderer in renderers) {
                    spriteRenderer.flipX = value;
                }
            }
        }
        private void OnEnable() {
            ColorizableRendererEnabledEvent.Invoke(this);
        }

        private void Awake() {
            renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        private void OnDisable() {
            ColorizableRendererDisabledEvent.Invoke(this);
        }

        public static readonly ColorizableRendererEvent
            ColorizableRendererEnabledEvent = new ColorizableRendererEvent();

        public static readonly ColorizableRendererEvent ColorizableRendererDisabledEvent =
            new ColorizableRendererEvent();
    }
}