using System;
using System.Collections.Generic;
using Datenshi.Scripts.Util;
using Shiroi.FX.Services;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Graphics {
    [ExecuteInEditMode]
    public class BlackAndWhiteFX : SingletonServiceController<BlackAndWhiteFX, BlackAndWhiteMeta> {
        private const string ShaderName = "Datenshi/BlackAndWhiteShader";
        private const string DarkenPropertyName = "_DarkenAmount";
        private const string PropertyName = "_Amount";
        public float DefaultDesaturationAmount = 0;
        public float DefaultDarkenAmount = 0;

        private float Amount {
            get {
                return Material != null ? Material.GetFloat(PropertyName) : 0;
            }
            set {
                if (Material != null) {
                    Material.SetFloat(PropertyName, value);
                }
            }
        }

        private float DarkenAmount {
            get {
                return Material != null ? Material.GetFloat(DarkenPropertyName) : 0;
            }
            set {
                if (Material != null) {
                    Material.SetFloat(DarkenPropertyName, value);
                }
            }
        }

        [SerializeField, ReadOnly]
        protected Material Material;

        public Material GetMaterial() {
            return Material ? Material : (Material = LoadMaterial());
        }

        private Material LoadMaterial() {
            var shader = Shader.Find(ShaderName);
            if (shader != null) {
                return new Material(shader);
            }

            Debug.LogWarningFormat("Couldn't find shader '{0}' for black and white effect", ShaderName);
            return null;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            var m = GetMaterial();
            if (m == null) {
                return;
            }

            UnityEngine.Graphics.Blit(source, destination, m);
        }

        protected override void UpdateGameToDefault() {
            DarkenAmount = DefaultDarkenAmount;
            Amount = DefaultDesaturationAmount;
        }

        protected override void UpdateGameTo(IEnumerable<WeightnedMeta<BlackAndWhiteMeta>> activeMetas) {
            float darken = 0, desaturate = 0;
            foreach (var weightedMeta in activeMetas) {
                var meta = weightedMeta.Meta;
                var w = weightedMeta.Weight;
                darken += meta.DarkenAmount * w;
                desaturate += meta.DesaturateAmount * w;
            }

            DarkenAmount = darken;
            Amount = desaturate;
        }

        protected override void UpdateGameTo(BlackAndWhiteMeta meta) {
            DarkenAmount = meta.DarkenAmount;
            Amount = meta.DesaturateAmount;
        }
    }

    public class BlackAndWhiteMeta : IComparable<BlackAndWhiteMeta>, ITimedServiceTickable {
        public AnimationCurve DesaturateCurve;
        public AnimationCurve DarkenCurve;

        public BlackAndWhiteMeta(AnimationCurve desaturateCurve, AnimationCurve darkenCurve) {
            DesaturateCurve = desaturateCurve;
            DarkenCurve = darkenCurve;
        }

        public float DesaturateAmount => DesaturateCurve.Evaluate(currentPos);
        public float DarkenAmount => DarkenCurve.Evaluate(currentPos);
        private float currentPos;

        public int CompareTo(BlackAndWhiteMeta other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var desaturateAmountComparison = DesaturateAmount.CompareTo(other.DesaturateAmount);
            return desaturateAmountComparison != 0
                ? desaturateAmountComparison
                : DarkenAmount.CompareTo(other.DarkenAmount);
        }

        public void Tick(ITimedService value) {
            currentPos = value.PercentageCompleted;
        }
    }
}