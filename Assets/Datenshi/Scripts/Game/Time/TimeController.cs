using Datenshi.Scripts.Util.Singleton;
using DG.Tweening;
using DG.Tweening.Core;

namespace Datenshi.Scripts.Game.Time {
    public class TimeController : Singleton<TimeController> {
        private static readonly DOGetter<float> Getter = TimeGetter;
        private static readonly DOSetter<float> Setter = TimeSetter;

        private static float TimeGetter() {
            return UnityEngine.Time.timeScale;
        }

        private static void TimeSetter(float scale) {
            UnityEngine.Time.timeScale = scale;
        }

        public float DefaultInitTimeScale = 1;
        public float DefaultSlowdownDuration = .25F;
        public void Slowdown() {
            Slowdown(DefaultInitTimeScale);
        }

        public void Slowdown(float initScale) {
            Slowdown(initScale, DefaultSlowdownDuration);
        }

        public void Slowdown(float initScale, float duration) {
            this.DOKill();
            UnityEngine.Time.timeScale = initScale;
            DOTween.To(Getter, Setter, 1, duration).SetUpdate(true);
        }
    }
}