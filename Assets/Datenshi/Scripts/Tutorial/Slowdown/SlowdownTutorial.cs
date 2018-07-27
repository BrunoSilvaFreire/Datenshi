using Datenshi.Scripts.Game.Time;
using Datenshi.Scripts.Util.Services;
using UnityEngine;

namespace Datenshi.Scripts.Tutorial.Slowdown {
    public abstract class SlowdownTutorialExecutor : MonoBehaviour {
        public float TimeScale;
        public abstract void Init(IndefiniteService<TimeMeta> meta);
        public abstract void Tick(IndefiniteService<TimeMeta> service);
    }

    public class SlowdownTutorial : Tutorial {
        public SlowdownTutorialExecutor Executor;
        private IndefiniteService<TimeMeta> service;

        protected override void OnStartTutorial() {
            service = TimeController.Instance.RequestIndefiniteSlowdown(Executor.TimeScale);
            Executor.Init(service);
        }

        protected override void OnStopTutorial() {
            if (service == null) {
                return;
            }

            service.Cancel();
            service = null;
        }

        private void Update() {
            if (service == null) {
                return;
            }

            Executor.Tick(service);
            if (service.IsFinished()) {
                service = null;
            }
        }
    }
}