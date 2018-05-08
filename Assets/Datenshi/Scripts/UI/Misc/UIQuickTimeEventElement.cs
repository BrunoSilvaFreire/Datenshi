using System.Collections;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;
using UPM.Input;

namespace Datenshi.Scripts.UI.Misc {
    public interface IQuickTimeEventController {
        bool Available {
            get;
            set;
        }
    }

    public class QuickTimeEventExecutor {
        public QuickTimeEventExecutor(IQuickTimeEventController controller, float timeScale, Vector2 uiOffset,
            ICombatant executor) {
            Controller = controller;
            TimeScale = timeScale;
            UIOffset = uiOffset;
            Executor = executor;
        }

        private IQuickTimeEventController Controller {
            get;
        }

        private Dispatcher Dispatcher {
            get;
            set;
        }

        private float TimeScale {
            get;
            set;
        }

        private Vector2 UIOffset {
            get;
        }

        private ICombatant Executor {
            get;
        }

        private float InitScale {
            get;
            set;
        }

        public void Execute(UnityAction onSuccess, UnityAction onFailed, int action, UIQuickTimeEventElement prefab) {
            Controller.Available = false;
            Dispatcher = Executor.Transform.GetOrAddComponent<Dispatcher>();
            Element = prefab.Clone(Executor.Transform.position + (Vector3) UIOffset);
            InitScale = Time.timeScale;
            Time.timeScale = TimeScale;
            Element.SnapShowing(false);
            Element.Show();
            Element.Play(delegate {
                onFailed?.Invoke();
                Reset(false);
            }, true);
            Dispatcher.StartCoroutine(WaitQTE(Executor.InputProvider, action, onSuccess));
        }

        private IEnumerator WaitQTE(InputProvider provider, int action, UnityAction onSuccess) {
            yield return null;
            while (Element.Counting) {
                if (provider.GetButtonDown(action)) {
                    onSuccess();
                    Reset(true);

                    yield break;
                }

                yield return null;
            }
        }

        private void Reset(bool sucess) {
            Time.timeScale = InitScale;
            Controller.Available = true;
            Object.Destroy(Dispatcher);
            Element.ResetCounter(sucess);
        }

        public UIQuickTimeEventElement Element {
            get;
            private set;
        }
    }

    public class UIQuickTimeEventElement : UIInputDisplay {
        public UnityEvent OnTimeEnded;
        public UICircle CompletionCircle;
        public Color SuccessColor;
        public Sprite SucessSprite;
        public float TotalTime;
        public float DarkenAmount;
        public float DarkenDuration;

        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private float currentTime;

        public bool Counting {
            get;
            private set;
        }


        public void ResetCounter(bool success = false, bool play = false) {
            Counting = false;
            currentTime = 0;
            OnTimeEnded.RemoveAllListeners();
            if (success) {
                Sprite = SucessSprite;
                CompletionCircle.Progress = 1;
                CompletionCircle.ProgressColor = SuccessColor;
            }

            if (play) {
                Play();
            }
        }

        public void Play(UnityAction action = null, bool reset = false) {
            if (reset) {
                ResetCounter();
            }

            Counting = true;
            PlayerController.Instance.Fx.DODarkenAmount(DarkenAmount, DarkenDuration);
            if (action != null) {
                OnTimeEnded.AddListener(action);
            }
        }

        private void Update() {
            if (!Counting) {
                return;
            }

            if (currentTime >= TotalTime) {
                Finish();
                return;
            }

            currentTime += Time.unscaledDeltaTime;
            var p = currentTime / TotalTime;
            CompletionCircle.SetProgress(p);
        }

        public void Stop() {
            StartCoroutine(DoStop());
        }

        private IEnumerator DoStop() {
            PlayerController.Instance.Fx.DODarkenAmount(0, DarkenDuration);
            Hide();
            ResetCounter();
            yield return new WaitForSeconds(FadeDuration);
            Destroy(gameObject);
        }

        private void Finish() {
            OnTimeEnded.Invoke();
            Stop();
        }
    }
}