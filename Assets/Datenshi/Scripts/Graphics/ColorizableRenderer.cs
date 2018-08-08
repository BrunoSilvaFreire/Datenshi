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

        [ShowInInspector, ReadOnly]
        private SpriteRenderer[] renderers;

        [ShowInInspector, ReadOnly]
        private TrailRenderer trails;

        [ShowInInspector, ReadOnly]
        private ColorizableSlave[] slaves;

        private MaterialPropertyBlock block;
        public bool Outline;
        public Color OutlineColor = Color.red;
        public float DefaultOverrideAmount;

        [ShowInInspector]
        private readonly ServiceHandler<ColorOverride> overrideServiceHandler = new ServiceHandler<ColorOverride>();

        [ShowInInspector]
        private readonly ServiceHandler<ColorOverride> mainServiceHandler = new ServiceHandler<ColorOverride>();

        public TimedService<ColorOverride> RequestColorOverrideNoUpdate(Color color, float amount, float duration,
            byte priority = Service.DefaultPriority) {
            return RequestColorOverride(new ColorOverride(color, amount, false), duration, priority);
        }

        public TimedService<ColorOverride> RequestMainColorOverrideNoUpdate(Color color, float amount, float duration,
            byte priority = Service.DefaultPriority) {
            return RequestMainColorOverride(new ColorOverride(color, amount, false), duration, priority);
        }

        private TimedService<ColorOverride> RequestMainColorOverride(ColorOverride meta, float duration,
            byte priority = Service.DefaultPriority) {
            return mainServiceHandler.RegisterTimedService(meta, duration, priority);
        }

        public TimedService<ColorOverride> RequestColorOverride(ColorOverride meta, float duration,
            byte priority = Service.DefaultPriority) {
            return overrideServiceHandler.RegisterTimedService(meta, duration, priority);
        }

        public TimedService<ColorOverride> RequestColorOverride(Color color, float amount, float duration,
            byte priority = Service.DefaultPriority) {
            return RequestColorOverride(new ColorOverride(color, amount), duration, priority);
        }

        public TimedService<ColorOverride> RequestMainColorOverride(Color color, float amount, float duration,
            byte priority = Service.DefaultPriority) {
            return RequestMainColorOverride(new ColorOverride(color, amount), duration, priority);
        }

        public static readonly ColorizableRendererEvent
            ColorizableRendererEnabledEvent = new ColorizableRendererEvent();

        public static readonly ColorizableRendererEvent ColorizableRendererDisabledEvent =
            new ColorizableRendererEvent();

        private void Awake() {
            ColorizableRendererEnabledEvent.Invoke(this);
            renderers = GetComponentsInChildren<SpriteRenderer>();
            slaves = new ColorizableSlave[renderers.Length];
            for (var i = 0; i < slaves.Length; i++) {
                var ren = renderers[i];
                var slave = ren.GetOrAddComponent<ColorizableSlave>();
                slave.Initialize(ren);
                slaves[i] = slave;
            }
        }

        private void OnDisable() {
            ColorizableRendererDisabledEvent.Invoke(this);
        }

        private Tweener impactTweener;

        private void Update() {
            overrideServiceHandler.Tick();
            mainServiceHandler.Tick();
            if (block == null) {
                block = new MaterialPropertyBlock();
            }

            var service = overrideServiceHandler.WithGenericHighestPriority();
            var spriteService = mainServiceHandler.WithGenericHighestPriority();
            if (renderers == null || slaves == null) {
                return;
            }

            for (var i = 0; i < renderers.Length; i++) {
                var r = renderers[i];
                if (r == null) {
                    continue;
                }

                if (i >= slaves.Length) {
                    continue;
                }

                var slave = slaves[i];

                var spriteColor = slave.SpriteColor;
                if (spriteService != null) {
                    var meta = spriteService.Metadata;
                    spriteColor = Color.Lerp(spriteColor, meta.Color, meta.Amount);
                }

                r.color = spriteColor;
                r.GetPropertyBlock(block);
                block.SetColor(OutlineColorKey, OutlineColor);
                block.SetFloat(OutlineKey, Outline ? 1 : 0);
                if (service != null) {
                    var meta = service.Metadata;
                    block.SetColor(OverrideColorKey, meta.Color);
                    block.SetFloat(OverrideAmountKey, meta.Amount);
                } else {
                    block.SetFloat(OverrideAmountKey, DefaultOverrideAmount);
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

        public void Tick(Service value) {
            var t = value as ITimedService;
            if (t == null || !Update) {
                return;
            }

            Amount = startAmount * (1 - t.Percentage);
        }
    }
}