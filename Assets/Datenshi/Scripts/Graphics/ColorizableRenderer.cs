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
        ColorizableRenderer ColorizableRenderer {
            get;
        }
    }

    [ExecuteInEditMode]
    public class ColorizableRenderer : ServiceController<ColorOverride> {
        public const string OverrideAmountKey = "_OverrideAmount";
        public const string OverrideColorKey = "_OverrideColor";
        public const string OutlineKey = "_Outline";
        public const string OutlineColorKey = "_OutlineColor";

        [ShowInInspector, ReadOnly]
        private SpriteRenderer[] renderers;

        private TrailRenderer[] trails;


        private MaterialPropertyBlock block;
        public bool Outline;
        public Color OutlineColor = Color.red;
        public float DefaultOverrideAmount;


        public static readonly ColorizableRendererEvent
            ColorizableRendererEnabledEvent = new ColorizableRendererEvent();

        public static readonly ColorizableRendererEvent ColorizableRendererDisabledEvent =
            new ColorizableRendererEvent();

        private void Awake() {
            ColorizableRendererEnabledEvent.Invoke(this);
            renderers = GetComponentsInChildren<SpriteRenderer>();
            trails = GetComponentsInChildren<TrailRenderer>();
        }

        private void OnDisable() {
            ColorizableRendererDisabledEvent.Invoke(this);
        }

        private Tweener impactTweener;
        private bool flipX;

        public bool FlipX {
            get {
                return flipX;
            }
            set {
                flipX = value;
                foreach (var spriteRenderer in renderers) {
                    spriteRenderer.flipX = value;
                }
            }
        }

        private void SetSpriteColor(Color overrideColor, float overrideAmount) {
            if (renderers == null || trails == null) {
                return;
            }

            if (block == null) {
                block = new MaterialPropertyBlock();
            }

            block.SetColor(OverrideColorKey, overrideColor);
            block.SetFloat(OverrideAmountKey, overrideAmount);
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
                block.SetColor(OutlineColorKey, OutlineColor);
                block.SetFloat(OutlineKey, Outline ? 1 : 0);
                r.SetPropertyBlock(block);
            }
        }

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


    public class TrailOverride : ITimedServiceTickable {
        public float Length;
        private readonly float startAmount;

        public float Amount {
            get;
            set;
        }

        public bool Update {
            get;
            set;
        }

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

        public float Amount {
            get;
            set;
        }

        public bool Update {
            get;
            set;
        }

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