using System.Collections.Generic;
using Shiroi.FX.Services;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Graphics.Colorization {
    public abstract class AbstractColorController<T> : ServiceController<T> {
        [ShowInInspector, ReadOnly]
        private SpriteRenderer[] renderers;

        [ShowInInspector, ReadOnly]
        private TrailRenderer[] trails;

        private MaterialPropertyBlock block;

        private void Start() {
            renderers = GetComponentsInChildren<SpriteRenderer>();
            trails = GetComponentsInChildren<TrailRenderer>();
        }

        protected abstract void ApplyToBlock(MaterialPropertyBlock block, T meta);

        private void SetColor(T meta) {
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
                ApplyToBlock(block, meta);
                r.SetPropertyBlock(block);
            }
        }

        protected override void UpdateGameToDefault() {
            SetColor(GetEmptyMeta());
        }

        protected abstract T GetEmptyMeta();
        protected abstract void Accumulate(ref T dest, T source, float weight);

        protected override void UpdateGameTo(IEnumerable<WeightnedMeta<T>> activeMetas) {
            var meta = GetEmptyMeta();
            foreach (var weightedMeta in activeMetas) {
                var m = weightedMeta.Meta;
                var w = weightedMeta.Weight;
                Accumulate(ref meta, m, w);
            }

            SetColor(meta);
        }

        protected override void UpdateGameTo(T meta) {
            SetColor(meta);
        }
    }

    public class ColorMeta : ITimedServiceTickable {
        private Color color;
        private readonly bool fadeAlphaOnTimedServices;
        private readonly float startAlpha;

        public ColorMeta(Color color, bool fadeAlphaOnTimedServices = true) {
            this.color = color;
            this.fadeAlphaOnTimedServices = fadeAlphaOnTimedServices;
            this.startAlpha = color.a;
        }

        public Color Color {
            get => color;
            set => color = value;
        }

        public void Tick(ITimedService service) {
            if (fadeAlphaOnTimedServices) {
                color.a = startAlpha * (1 - service.PercentageCompleted);
            }
        }
    }
}