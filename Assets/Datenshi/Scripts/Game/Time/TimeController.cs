using System.Collections;
using System.Collections.Generic;
using Datenshi.Scripts.Util.Singleton;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

namespace Datenshi.Scripts.Game.Time {
    public class Slowdown {
        private readonly TimeController controller;

        public float Duration {
            get;
            private set;
        }

        internal Slowdown(float duration, TimeController controller) {
            IsActive = true;
            Duration = duration;
            this.controller = controller;
        }

        public bool IsActive {
            get;
            private set;
        }

        public void Stop() {
            if (!IsActive) {
                return;
            }

            if (controller.CurrentSlowdown == this) {
                controller.ResetTime();
                controller.CurrentSlowdown = null;
            }

            IsActive = false;
        }
    }

    public class TimeController : Singleton<TimeController> {
        public float DefaultTimeScale = 1;
        public float DefaultSlowdownScale = .25F;
        public float DefaultImpactFrameScale = .5F;
        public float DefaultImpactDuration = .25F;
        public float TimeChangeDuration = .25F;
        private Tween currentTween;
        private static readonly DOGetter<float> Getter = TimeGetter;
        private static readonly DOSetter<float> Setter = TimeSetter;
        internal Slowdown CurrentSlowdown;

        
        private static float TimeGetter() {
            return UnityEngine.Time.timeScale;
        }

        private static void TimeSetter(float scale) {
            UnityEngine.Time.timeScale = scale;
        }

        public void Reset() {
            currentTween?.Kill();
            currentTween = null;
        }

        public void ImpactFrame() {
            ImpactFrame(DefaultImpactDuration);
        }

        public void ImpactFrame(float duration) {
            UnityEngine.Time.timeScale = DefaultImpactFrameScale;
            DOTime(DefaultTimeScale, duration);
        }

        public Slowdown Slowdown(float duration) {
            return Slowdown(duration, DefaultSlowdownScale);
        }

        public Slowdown Slowdown(float duration, float timeScale) {
            var s = CurrentSlowdown;
            CurrentSlowdown = new Slowdown(duration, this);
            s?.Stop();
            StartCoroutine(Wait(CurrentSlowdown, CurrentSlowdown.Duration));
            return CurrentSlowdown;
        }

        private IEnumerator Wait(Slowdown slowdown, float slowdownDuration) {
            yield return new WaitForSecondsRealtime(slowdownDuration);
            if (slowdown.IsActive) {
                slowdown.Stop();
            }
        }

        private void DOTime(float scale, float duration) {
            Reset();
            currentTween = DOTween.To(Getter, Setter, scale, duration);
            currentTween.timeScale = 1;
            currentTween.OnComplete(Reset);
        }

        public void ResetTime() {
            DOTime(DefaultTimeScale, TimeChangeDuration);
        }
    }
}