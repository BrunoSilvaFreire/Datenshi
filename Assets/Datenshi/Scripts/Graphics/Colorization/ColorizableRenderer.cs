using System;
using System.Collections.Generic;
using Datenshi.Scripts.Util;
using DG.Tweening;
using Shiroi.FX.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Graphics {
    [Serializable]
    public sealed class ColorizableRendererEvent : UnityEvent<ColorizableRenderer> { }

    public interface IColorizable {
        ColorizableRenderer ColorizableRenderer { get; }
    }

    public sealed class Outline : ITimedServiceTickable {
        private Color color;
        private readonly bool fadeAlphaOnTimedServices;
        private readonly float startAlpha;

        public Outline(Color color, bool fadeAlphaOnTimedServices = true) {
            this.color = color;
            this.fadeAlphaOnTimedServices = fadeAlphaOnTimedServices;
            this.startAlpha = color.a;
        }

        public Color Color => color;

        public void Tick(ITimedService service) {
            if (fadeAlphaOnTimedServices) {
                color.a = startAlpha * (1 - service.PercentageCompleted);
            }
        }
    }

    public class OutlineController : ServiceController<Outline> {
        public const string OutlineColorKey = "_OutlineColor";

        [ShowInInspector, ReadOnly]
        private SpriteRenderer[] renderers;

        [ShowInInspector, ReadOnly]
        private TrailRenderer[] trails;

        private MaterialPropertyBlock block;

        private void Start() {
            renderers = GetComponentsInChildren<SpriteRenderer>();
            trails = GetComponentsInChildren<TrailRenderer>();
        }

        private void SetColor(Color color) {
            if (renderers == null || trails == null) {
                return;
            }

            if (block == null) {
                block = new MaterialPropertyBlock();
            }

            foreach (var trail in trails) {
                if (trail == null) {
                    continue;
                }

                trail.SetPropertyBlock(block);
            }

            foreach (var r in renderers) {
                if (r == null) {
                    continue;
                }

                r.GetPropertyBlock(block);
                block.SetColor(OutlineColorKey, color);
                r.SetPropertyBlock(block);
            }
        }

        protected override void UpdateGameToDefault() {
            SetColor(Color.clear);
        }

        protected override void UpdateGameTo(IEnumerable<WeightnedMeta<Outline>> activeMetas) {
            var color = Color.clear;
            foreach (var weightnedMeta in activeMetas) {
                var m = weightnedMeta.Meta;
                var w = weightnedMeta.Weight;
                color += w * m.Color;
            }

            SetColor(color);
        }

        protected override void UpdateGameTo(Outline meta) {
            SetColor(meta.Color);
        }
    }

    public class ColorController : ServiceController<ColorOverride> {
        public float DefaultOverrideAmount;
        [ShowInInspector, ReadOnly]
        private SpriteRenderer[] renderers;

        [ShowInInspector, ReadOnly]
        private TrailRenderer[] trails;

        private MaterialPropertyBlock block;

        private void Awake() {
            renderers = GetComponentsInChildren<SpriteRenderer>();
            trails = GetComponentsInChildren<TrailRenderer>();
        }

        private void OnDisable() {
            ColorizableRendererDisabledEvent.Invoke(this);
        }

        private Tweener impactTweener;
        private bool flipX;

        public bool FlipX {
            get { return flipX; }
            set {
                flipX = value;
                foreach (var spriteRenderer in renderers) {
                    spriteRenderer.flipX = value;
                }
            }
        }

        private void SetSpriteColor(Color overrideColor, float overrideAmount) { }

        protected override void UpdateGameToDefault() {
            SetSpriteColor(Color.clear, DefaultOverrideAmount);
        }

        protected override void UpdateGameTo(IEnumerable<WeightnedMeta<ColorOverride>> activeMetas) {
            var color = Color.clear;
            float amount = 0;
            foreach (var weightnedMeta in activeMetas) {
                var m = weightnedMeta.Meta;
                var w = weightnedMeta.Weight;
                color += w * m.Color;
                amount += w * m.Amount;
            }

            SetSpriteColor(color, amount);
        }

        protected override void UpdateGameTo(ColorOverride meta) {
            SetSpriteColor(meta.Color, meta.Amount);
        }
    }

    [ExecuteInEditMode]
    public class ColorizableRenderer : MonoBehaviour {
        public const string OverrideAmountKey = "_OverrideAmount";
        public const string OverrideColorKey = "_OverrideColor";
        public OutlineController OutlineController;

        private MaterialPropertyBlock block;
        public Color OutlineColor = Color.red;


        public static readonly ColorizableRendererEvent
            ColorizableRendererEnabledEvent = new ColorizableRendererEvent();

        public static readonly ColorizableRendererEvent ColorizableRendererDisabledEvent =
            new ColorizableRendererEvent();
    }


    public class TrailOverride : ITimedServiceTickable {
        public float Length;
        private readonly float startAmount;

        public float Amount { get; set; }

        public bool Update { get; set; }

        public TrailOverride(float length, bool update = true) {
            Length = length;
            Update = update;
        }

        public int CompareTo(TrailOverride other) {
            if (ReferenceEquals(this, other)) return 0;
            return ReferenceEquals(null, other) ? 1 : Amount.CompareTo(other.Amount);
        }

        public void Tick(ITimedService service) {
            if (!Update) {
                return;
            }

            Amount = startAmount * (1 - service.PercentageCompleted);
        }
    }

    public class ColorOverride : IComparable<ColorOverride>, ITimedServiceTickable {
        public Color Color;
        private readonly float startAmount;

        public float Amount { get; set; }

        public bool Update { get; set; }

        public ColorOverride(Color color, float amount, bool update = true) {
            Color = color;
            Amount = amount;
            Update = update;
            startAmount = amount;
        }

        public int CompareTo(ColorOverride other) {
            if (ReferenceEquals(this, other)) return 0;
            return ReferenceEquals(null, other) ? 1 : Amount.CompareTo(other.Amount);
        }

        public void Tick(ITimedService service) {
            if (!Update) {
                return;
            }

            Amount = startAmount * (1 - service.PercentageCompleted);
        }
    }
}