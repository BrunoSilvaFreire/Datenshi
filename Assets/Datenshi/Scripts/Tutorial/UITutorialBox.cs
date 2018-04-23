using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.UI;
using Datenshi.Scripts.Util;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Tutorial {
    public class UITutorialBox : UIElement {
        private static UITutorialBox instance;

        public static UITutorialBox Instance {
            get {
                return instance ?? (instance = FindObjectOfType<UITutorialBox>());
            }
        }

        public float SizeChangeDuration;

        public CanvasGroup CanvasGroup;
        public RectTransform Holder;
        public Vector2 DefaultSize;
        public Vector2 Padding;

        private RectTransform RectTransform {
            get {
                return (RectTransform) transform;
            }
        }

        private void Start() {
            SnapHide();
        }

        protected override void SnapShow() {
            CanvasGroup.alpha = 1;
            RectTransform.sizeDelta = (current != null ? current.Size : DefaultSize) + Padding;
        }

        protected override void SnapHide() {
            CanvasGroup.alpha = 0;
            RectTransform.sizeDelta = Vector2.zero;
        }

        protected override void OnShow() { }

        protected override void OnHide() {
            hideRoutine = StartCoroutine(HideTutoral());
        }

        public void Show(TutorialTrigger tutorialTrigger) {
            if (knownTutorials.Contains(tutorialTrigger)) {
                return;
            }
            if (!Showing) {
                Showing = true;
            }

            knownTutorials.Add(tutorialTrigger);
            showRoutine = StartCoroutine(ShowTutorial(tutorialTrigger));
        }

        public void Deregister(TutorialTrigger id) {
            knownTutorials.Remove(id);
            if (id != currentTrigger) {
                return;
            }
            if (knownTutorials.IsEmpty()) {
                Hide();
            } else {
                var toReplace = knownTutorials.Last();
                showRoutine = StartCoroutine(ShowTutorial(toReplace));
            }
        }

        private IEnumerator HideTutoral() {
            if (showRoutine != null) {
                Stop(showRoutine);
            }

            if (current != null) {
                yield return Clear(current);
            }

            yield return SetSizeAndAlpha(Vector2.zero, 0);
            hideRoutine = null;
        }

        private void Stop(Coroutine coroutine) {
            RectTransform.DOKill();
            CanvasGroup.DOKill();
            StopCoroutine(coroutine);
        }


        private IEnumerator SetSizeAndAlpha(Vector2 size, float alpha) {
            RectTransform.DOSizeDelta(size + Padding, SizeChangeDuration, true);
            CanvasGroup.DOFade(alpha, SizeChangeDuration);
            yield return new WaitForSeconds(SizeChangeDuration);
        }

        [ShowInInspector]
        private readonly List<TutorialTrigger> knownTutorials = new List<TutorialTrigger>();

        private TutorialTrigger currentTrigger;
        private UITutorial current;
        private Coroutine hideRoutine;
        private Coroutine showRoutine;

        private IEnumerator ShowTutorial(TutorialTrigger tutorialTrigger) {
            if (hideRoutine != null) {
                Stop(hideRoutine);
            }

            if (current != null) {
                yield return Clear(current);
            }

            var tut = Instantiate(tutorialTrigger.TutorialPrefab, Holder);
            tut.SnapShowing(false);
            yield return SetSizeAndAlpha(tut.Size, 1);
            tut.Showing = true;
            currentTrigger = tutorialTrigger;
            current = tut;
            showRoutine = null;
        }


        private IEnumerator Clear(UITutorial obj) {
            if (obj == null) {
                yield break;
            }

            if (obj == current) {
                current = null;
                currentTrigger = null;
            }

            obj.Showing = false;
            yield return new WaitForSeconds(obj.FadeDuration);
            Destroy(obj.gameObject);
        }
    }
}