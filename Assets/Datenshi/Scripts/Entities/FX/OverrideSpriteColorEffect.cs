using System;
using System.Collections;
using System.Collections.Generic;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Util.Services;
using UnityEngine;

namespace Datenshi.Scripts.Entities.FX {
    [CreateAssetMenu(menuName = BasePath + "/OverrideColor")]
    public class OverrideSpriteColorEffect : EntityEffect {
        [Flags]
        public enum OverrideMode : byte {
            Main = 1 << 0,
            Override = 1 << 1
        }

        public OverrideMode Mode;
        public Gradient Color;
        public float Duration;
        public AnimationCurve Amount = AnimationCurve.Constant(0, 1, 1);

        public override void Execute(Entity entity) {
            var services = new List<Service<ColorOverride>>();
            var initColor = Color.Evaluate(0);
            var initAmount = Amount.Evaluate(0);
            if ((Mode & OverrideMode.Main) == OverrideMode.Main) {
                var service = entity.ColorizableRenderer.RequestColorOverrideNoUpdate(initColor, initAmount, Duration);
                services.Add(service);
            }

            if ((Mode & OverrideMode.Override) == OverrideMode.Override) {
                var service =
                    entity.ColorizableRenderer.RequestMainColorOverrideNoUpdate(initColor, initAmount, Duration);
                services.Add(service);
            }

            entity.StartCoroutine(ExecuteGradient(services));
        }

        private IEnumerator ExecuteGradient(IReadOnlyCollection<Service<ColorOverride>> services) {
            var left = Duration;
            while (left > 0) {
                left -= Time.deltaTime;
                var currentPos = 1 - left / Duration;
                var currentColor = Color.Evaluate(currentPos);
                var currentAmount = Amount.Evaluate(currentPos);
                foreach (var service in services) {
                    var meta = service.Metadata;
                    meta.Amount = currentAmount;
                    meta.Color = currentColor;
                }

                yield return null;
            }
        }
    }
}