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

        private IEnumerator HideTutoral() {
            if (showRoutine != null) {
                StopCoroutine(showRoutine);
            }

            if (current != null) {
                yield return StartCoroutine(Clear(current));
            }

            yield return SetSizeAndAlpha(Vector2.zero, 0);
            hideRoutine = null;
        }

        private RectTransform RectTransform {
            get {
                return (RectTransform) transform;
            }
        }

        private IEnumerator SetSizeAndAlpha(Vector2 size, float alpha) {
            RectTransform.DOKill();
            CanvasGroup.DOKill();
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

        public void Show(TutorialTrigger tutorialTrigger) {
            if (!Showing) {
                Showing = true;
            }

            knownTutorials.Add(tutorialTrigger);
            showRoutine = StartCoroutine(ShowTutorial(tutorialTrigger));
        }


        private IEnumerator ShowTutorial(TutorialTrigger tutorialTrigger, bool destroy = false) {
            if (hideRoutine != null) {
                StopCoroutine(hideRoutine);
            }

            if (current != null) {
                yield return StartCoroutine(Clear(current, destroy));
            }

            var tut = Instantiate(tutorialTrigger.TutorialPrefab, Holder);
            tut.SnapShowing(false);
            yield return SetSizeAndAlpha(tut.Size, 1);
            tut.Showing = true;
            currentTrigger = tutorialTrigger;
            current = tut;
            showRoutine = null;
        }


        private IEnumerator Clear(UITutorial obj, bool destroy = true) {
            if (obj == null) {
                yield break;
            }

            if (obj == current) {
                current = null;
                currentTrigger = null;
            }

            obj.Showing = false;
            yield return new WaitForSeconds(obj.FadeDuration);
            if (destroy) {
                Destroy(obj.gameObject);
            }
        }

        public void Deregister(TutorialTrigger id) {
            knownTutorials.Remove(id);
            if (id != currentTrigger) {
                return;
            }

            current.DOKill();
            if (knownTutorials.Count == 0) {
                Hide();
            } else {
                var toReplace = knownTutorials.Last();
                showRoutine = StartCoroutine(ShowTutorial(toReplace, true));
            }
        }
    }
}