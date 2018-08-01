using System;
using Datenshi.Scripts.Util.Services;
using Datenshi.Scripts.Util.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Util.Time {
    public class TimeController : Singleton<TimeController> {
        [ShowInInspector]
        public ServiceHandler<AnimatedTimeMeta> animatedTimeScaleHandler = new ServiceHandler<AnimatedTimeMeta>();

        [ShowInInspector]
        public ServiceHandler<TimeMeta> timeScaleHandler = new ServiceHandler<TimeMeta>();

        private float GetTimeScale() {
            float highest = 1;
            CheckHigher(ref highest, animatedTimeScaleHandler.WithGenericHighestPriority()?.Metadata);
            CheckHigher(ref highest, timeScaleHandler.WithGenericHighestPriority()?.Metadata);
            return highest;
        }

        private void CheckHigher(ref float highest, AbstractTimeMeta h) {
            if (h == null) {
                return;
            }

            if (h.TimeScale < highest) {
                highest = h.TimeScale;
            }
        }

        private void Update() {
            animatedTimeScaleHandler.Tick();
            timeScaleHandler.Tick();
            UnityEngine.Time.timeScale = GetTimeScale();
        }

        public TimedService<AnimatedTimeMeta> RequestAnimatedSlowdown(AnimationCurve timeScale, float duration,
            byte priority = Service.DefaultPriority) {
            var meta = new AnimatedTimeMeta(timeScale);
            return animatedTimeScaleHandler.RegisterTimedService(meta, duration, priority);
        }

        public TimedService<TimeMeta> RequestSlowdown(float timeScale, float duration,
            byte priority = Service.DefaultPriority) {
            var meta = new TimeMeta(timeScale);
            return timeScaleHandler.RegisterTimedService(meta, duration, priority);
        }

        public IndefiniteService<TimeMeta> RequestIndefiniteSlowdown(float timeScale,
            byte priority = Service.DefaultPriority) {
            var meta = new TimeMeta(timeScale);
            return timeScaleHandler.RegisterIndefiniteService(meta, priority);
        }
    }

    public abstract class AbstractTimeMeta : IComparable<AbstractTimeMeta> {
        public abstract float TimeScale {
            get;
        }

        public int CompareTo(AbstractTimeMeta other) {
            if (ReferenceEquals(this, other)) {
                return 0;
            }

            if (ReferenceEquals(null, other)) {
                return 1;
            }

            return -TimeScale.CompareTo(other.TimeScale);
        }
    }

    public class AnimatedTimeMeta : AbstractTimeMeta, ITickable<Service> {
        public AnimatedTimeMeta(AnimationCurve timeScaleCurve) {
            TimeScaleCurve = timeScaleCurve;
        }

        public readonly AnimationCurve TimeScaleCurve;

        public override float TimeScale => TimeScaleCurve.Evaluate(currentPosition);

        private float currentPosition;

        public void Tick(Service value) {
            var timed = value as ITimedService;
            if (timed == null) {
                return;
            }

            currentPosition = timed.Percentage;
        }
    }

    public class TimeMeta : AbstractTimeMeta {
        public float Scale;

        public override float TimeScale => Scale;

        public TimeMeta(float scale) {
            Scale = scale;
        }
    }
}