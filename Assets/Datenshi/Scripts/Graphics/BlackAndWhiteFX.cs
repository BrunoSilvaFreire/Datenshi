using System;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Services;
using UnityEngine;

namespace Datenshi.Scripts.Graphics {
    [ExecuteInEditMode]
    public class BlackAndWhiteFX : StandaloneVisualFX {
        private const string ShaderName = "Datenshi/BlackAndWhiteShader";
        private const string DarkenPropertyName = "_DarkenAmount";
        private const string PropertyName = "_Amount";

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

        private void Update() {
            serviceHandler.Tick();
            var highest = serviceHandler.WithGenericHighestPriority();
            if (highest == null) {
                Amount = 0;
                DarkenAmount = 0;
                return;
            }

            var meta = highest.Metadata;
            Amount = meta.DesaturateAmount;
            DarkenAmount = meta.DarkenAmount;
        }

        protected override string GetShaderName() {
            return ShaderName;
        }

        public TimedService<BlackAndWhiteMeta> RequestService(float duration, AnimationCurve desaturate,
            AnimationCurve darken,
            byte priority = Service.DefaultPriority) {
            var meta = new BlackAndWhiteMeta(
                desaturate, darken
            );
            return serviceHandler.RegisterTimedService(meta, duration, priority);
        }

        private readonly ServiceHandler<BlackAndWhiteMeta> serviceHandler = new ServiceHandler<BlackAndWhiteMeta>();
    }

    public class BlackAndWhiteMeta : IComparable<BlackAndWhiteMeta>, ITickable<Service> {
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

        public void Tick(Service value) {
            var timed = value as ITimedService;
            if (timed == null) {
                return;
            }

            currentPos = timed.Percentage;
        }
    }
}