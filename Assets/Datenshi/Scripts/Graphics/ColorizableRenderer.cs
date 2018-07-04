using System;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Services;
using DG.Tweening;
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
    public class ColorizableRenderer : MonoBehaviour {
        public const string OverrideAmountKey = "_OverrideAmount";
        public const string OverrideColorKey = "_OverrideColor";
        public const string OutlineKey = "_Outline";
        public const string OutlineColorKey = "_OutlineColor";
        private Renderer[] renderers;
        private MaterialPropertyBlock block;
        public bool Outline;
        public Color OutlineColor = Color.red;

        [ShowInInspector]
        private readonly ServiceHandler<ColorOverride> serviceHandler = new ServiceHandler<ColorOverride>();

        public TimedService<ColorOverride> RequestColorOverride(Color color, float amount, float duration,
            byte priority = Service.DefaultPriority) {
            var meta = new ColorOverride(color, amount);
            return serviceHandler.RegisterTimedService(meta, duration, priority);
        }

        public static readonly ColorizableRendererEvent
            ColorizableRendererEnabledEvent = new ColorizableRendererEvent();

        public static readonly ColorizableRendererEvent ColorizableRendererDisabledEvent =
            new ColorizableRendererEvent();

        private void Awake() {
            ColorizableRendererEnabledEvent.Invoke(this);
            renderers = GetComponentsInChildren<Renderer>();
        }

        private void OnDisable() {
            ColorizableRendererDisabledEvent.Invoke(this);
        }

        private Tweener impactTweener;

        private void Update() {
            serviceHandler.Tick();
            if (block == null) {
                block = new MaterialPropertyBlock();
            }

            var service = serviceHandler.WithGenericHighestPriority();
            foreach (var r in renderers) {
                if (r == null) {
                    continue;
                }

                r.GetPropertyBlock(block);
                block.SetColor(OutlineColorKey, OutlineColor);
                block.SetFloat(OutlineKey, Outline ? 1 : 0);
                if (service != null) {
                    var meta = service.Metadata;
                    block.SetColor(OverrideColorKey, meta.Color);
                    block.SetFloat(OverrideAmountKey, meta.Amount);
                } else {
                    block.SetFloat(OverrideAmountKey, 0);
                }

                r.SetPropertyBlock(block);
            }
        }
    }

    public class ColorOverride : IComparable<ColorOverride>, ITickable<Service> {
        public Color Color;
        private readonly float startAmount;

        public float Amount {
            get;
            private set;
        }

        public ColorOverride(Color color, float amount) {
            Color = color;
            Amount = amount;
            startAmount = amount;
        }

        public int CompareTo(ColorOverride other) {
            if (ReferenceEquals(this, other)) return 0;
            return ReferenceEquals(null, other) ? 1 : Amount.CompareTo(other.Amount);
        }

        public void Tick(Service value) {
            var t = value as ITimedService;
            if (t == null) {
                return;
            }

            Amount = startAmount * (1 - t.Percentage);
        }
    }
}