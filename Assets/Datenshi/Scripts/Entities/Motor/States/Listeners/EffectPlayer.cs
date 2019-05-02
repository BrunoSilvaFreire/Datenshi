using System;
using System.Collections.Generic;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motor.States.Listeners {
    [Serializable]
    public sealed class ScheduledEffect {
        public Effect Effect;
        public Vector2 PositionOffset;
    }

    public class EffectPlayer : MovementListener {
        public List<ScheduledEffect> Enter;
        public List<ScheduledEffect> Exit;

        private static void Play(List<ScheduledEffect> fxs, Entity entity) {
            foreach (var effect in fxs) {
                effect.Effect.Play(new EffectContext(entity,
                        new PositionFeature(
                            (Vector2) entity.transform.position + effect.PositionOffset)
                    )
                );
            }
        }

        public override void OnEnter(MovableEntity entity, StateMotor motor) {
            Play(Enter, entity);
        }

        public override void OnExit(MovableEntity entity, StateMotor motor) {
            Play(Exit, entity);
        }
    }
}