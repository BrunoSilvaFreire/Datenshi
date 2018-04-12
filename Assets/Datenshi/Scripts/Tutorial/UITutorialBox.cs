using System.Collections;
using Datenshi.Scripts.UI;
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

        [ShowInInspector]
        private uint inContact;

        private void Start() {
            SnapHide();
        }

        protected override void SnapShow() {
            CanvasGroup.alpha = 1;
            RectTransform.sizeDelta = (last != null ? last.Size : DefaultSize) + Padding;
        }

        protected override void SnapHide() {
            CanvasGroup.alpha = 0;
            RectTransform.sizeDelta = Vector2.zero;
        }

        protected override void OnShow() {
            StartCoroutine(ShowTutorial());
        }

        protected override void OnHide() {
            StartCoroutine(HideTutoral());
        }

        private IEnumerator HideTutoral() {
            yield return ClearLast();
            if (last != null) {
                Destroy(last.gameObject);
            }
            yield return SetSizeAndAlpha(Vector2.zero, 0);
        }

        private RectTransform RectTransform {
            get {
                return ((RectTransform) transform);
            }
        }

        private IEnumerator SetSizeAndAlpha(Vector2 size, float alpha) {
            RectTransform.DOKill();
            CanvasGroup.DOKill();
            RectTransform.DOSizeDelta(size + Padding, SizeChangeDuration, true);
            CanvasGroup.DOFade(alpha, SizeChangeDuration);
            yield return new WaitForSeconds(SizeChangeDuration);
        }

        public void Show(UITutorial uiTutorial) {
            inContact++;
            toReplace = uiTutorial;
            if (!Showing) {
                Showing = true;
            } else {
                StartCoroutine(ShowTutorial());
            }
        }


        private UITutorial last;
        private UITutorial toReplace;

        private IEnumerator ShowTutorial() {
            if (toReplace == null) {
                yield return SetSizeAndAlpha(last != null ? last.Size : DefaultSize, 1);
            } else {
                if (last != null) {
                    yield return ClearLast();
                }
                yield return SetSizeAndAlpha(toReplace.Size, 1);
                var tut = Instantiate(toReplace, Holder);
                tut.SnapShowing(false);
                tut.Showing = true;
                last = tut;
            }
        }


        private IEnumerator ClearLast() {
            last.Showing = false;
            yield return new WaitForSeconds(last.FadeDuration);
        }

        public void Deregister(UITutorial uiTutorialPrefab) {
            inContact--;
            if (inContact == 0) {
                Hide();
            }
        }
    }
}