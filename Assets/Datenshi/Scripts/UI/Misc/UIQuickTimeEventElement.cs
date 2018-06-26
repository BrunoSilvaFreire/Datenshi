using System;
using System.Collections;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.UI.Input;
using Datenshi.Scripts.Util;
using DG.Tweening;
using Rewired;
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

    public class UIQuickTimeEventElement : UIInputDisplay {
        public UnityAction OnFailed;
        public UnityAction OnSucess;
        public InputIconDatabase IconDatabase;
        public UICircle CompletionCircle;
        public Color SuccessColor;
        public Sprite SucessSprite;
        public float TotalTime;
        public float DarkenAmount;
        public float DarkenDuration;
        public float FinishStayDuration;
        public float LowPass = 400;
        public float NormalPass = 22000;

        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private float currentTime;

        private DatenshiInputProvider receiver;
        private int action;
        private bool inverted;

        public bool Counting {
            get;
            private set;
        }


        public void ResetCounter(bool success = false) {
            currentTime = 0;
            SetFrequency(NormalPass);
            if (!success) {
                return;
            }

            Image.transform.rotation = Quaternion.identity;
            Sprite = SucessSprite;
            CompletionCircle.Progress = 1;
            CompletionCircle.ProgressColor = SuccessColor;
        }

        private void SetFrequency(float f) {
            var filter = AudioManager.Instance.LowPassFilter;
            filter.DOKill();
            filter.DOFrequency(f, DarkenDuration);
        }

        public void Play(DatenshiInputProvider r, int a, bool inv, UnityAction onSucess, UnityAction onFailed) {
            var i = IconDatabase.GetIconFor(a);
            if (i == null) {
                Debug.Log("Icon is null for action " + a);
                return;
            }

            receiver = r;
            action = a;
            inverted = inv;
            OnSucess = onSucess;
            OnFailed = onFailed;
            ResetCounter();

            i.Setup(this, inv);
            SetFrequency(LowPass);
            //GraphicsSingleton.Instance.BlackAndWhite.DODarkenAmount(DarkenAmount, DarkenDuration);
            Counting = true;
            StartCoroutine(WaitForInput());
        }

        private IEnumerator WaitForInput() {
            yield return null;
            var selector = GetSelector(action);
            while (Counting) {
                if (selector(receiver, action, inverted)) {
                    Debug.Log("Selector" + selector + " return true for " + action + " @ " + inverted);
                    yield return null;
                    Finish(true);
                    yield break;
                }

                yield return null;
            }
        }

        public delegate bool InputChecker(InputProvider provider, int action, bool inverted);

        public static readonly InputChecker ButtonChecker = InputButtonChecker;


        private static bool InputButtonChecker(InputProvider provider, int i, bool inverted) {
            var b = provider.GetButtonDown(i);
            var r = inverted ? !b : b;
            Debug.Log($"Amount @ {i} = {b} @ {inverted} = {r}");
            return r;
        }

        public static readonly InputChecker AxisChecker = InputAxisChecker;
        public const float AxisLimit = .5F;

        private static bool InputAxisChecker(InputProvider provider, int i, bool inverted) {
            var amount = provider.GetAxis(i);
            if (inverted) {
                return amount < -AxisLimit;
            }

            return amount > AxisLimit;
        }


        private InputChecker GetSelector(int i) {
            var a = ReInput.mapping.GetAction(action);
            switch (a.type) {
                case InputActionType.Axis:
                    return AxisChecker;
                case InputActionType.Button:
                    return ButtonChecker;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update() {
            if (!Counting) {
                return;
            }

            if (currentTime >= TotalTime) {
                Finish(false);
                return;
            }

            currentTime += Time.unscaledDeltaTime;
            var p = currentTime / TotalTime;
            CompletionCircle.SetProgress(p);
        }


        private IEnumerator DoStop(bool success) {
            //GraphicsSingleton.Instance.BlackAndWhite.DODarkenAmount(0, DarkenDuration);
            ResetCounter(success);
            yield return new WaitForSeconds(FinishStayDuration);
            Hide();
            yield return new WaitForSeconds(HideDuration);
            Destroy(gameObject);
        }

        public void Finish(bool success) {
            Counting = false;
            if (success) {
                OnSucess();
            } else {
                OnFailed();
            }

            StartCoroutine(DoStop(success));
        }
    }
}