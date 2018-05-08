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

        public static UITutorialBox Instance => instance ? instance : (instance = FindObjectOfType<UITutorialBox>());

        public float SizeChangeDuration;

        public CanvasGroup CanvasGroup;
        public RectTransform Holder;
        public Vector2 DefaultSize;
        public Vector2 Padding;
        public float YOffset = .25F;
        private RectTransform RectTransform => (RectTransform) transform;

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
            StartCoroutine(HideTutoral());
        }

        public void Show(TutorialTrigger tutorialTrigger) {
            if (!Showing) {
                Showing = true;
            }

            if (!knownTutorials.Contains(tutorialTrigger)) {
                knownTutorials.Add(tutorialTrigger);
            }

            StartCoroutine(ShowTutorial(tutorialTrigger));
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
                StartCoroutine(ShowTutorial(toReplace));
            }
        }

        private IEnumerator HideTutoral() {
            hideScheduled = true;
            if (current != null) {
                yield return Clear(current);
            }

            if (showScheduled) {
                showScheduled = false;
                Kill();
            }

            if (hideScheduled) {
                yield return SetSizeAndAlpha(Vector2.zero, 0);
            }
        }

        private void Kill() {
            RectTransform.DOKill();
            CanvasGroup.DOKill();
        }


        private IEnumerator SetSizeAndAlpha(Vector2 size, float alpha) {
            RectTransform.DOAnchorPosY(size.y / 2 * transform.localScale.y + YOffset, SizeChangeDuration);
            RectTransform.DOSizeDelta(size + Padding, SizeChangeDuration, true);
            CanvasGroup.DOFade(alpha, SizeChangeDuration);
            yield return new WaitForSeconds(SizeChangeDuration);
        }

        [ShowInInspector]
        private readonly List<TutorialTrigger> knownTutorials = new List<TutorialTrigger>();

        private TutorialTrigger currentTrigger;
        private UITutorial current;
        private bool showScheduled;
        private bool hideScheduled;

        private IEnumerator ShowTutorial(TutorialTrigger tutorialTrigger) {
            showScheduled = true;
            if (hideScheduled) {
                hideScheduled = false;
                Kill();
            }

            if (current != null) {
                yield return Clear(current);
            }

            var tut = Instantiate(tutorialTrigger.TutorialPrefab, Holder);
            tut.SnapShowing(false);
            if (showScheduled) {
                yield return SetSizeAndAlpha(tut.Size, 1);
            }

            showScheduled = false;
            tut.Showing = true;
            currentTrigger = tutorialTrigger;
            current = tut;
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