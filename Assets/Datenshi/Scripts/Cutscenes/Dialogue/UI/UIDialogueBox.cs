using System.Collections;
using Datenshi.Scripts.UI;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Misc.Narrator;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.Cutscenes.Dialogue.UI {
    public class UIDialogueBox : UIView {
        public CanvasGroup CanvasGroup;
        public Text LabelField;
        public Text TextField;
        public float FadeDuration;

        public VerticalLayoutGroup LayoutGroup;
        public Narrator Narrator;
        public Transform ToFollow;
        public Vector2 ViewportPos;
        public float TextWidth;
        public int TitleFontSize;
        public float SizeTransferDuration = .5F;
        public float AlphaTransferDuration = .25F;
        public float BottomHeight = .25F;

        public IEnumerator Type(DialogueSpeech text) {
            foreach (var line in text.Lines) {
                yield return null;
            }
        }

        protected override void SnapShow() {
            CanvasGroup.alpha = 1;
        }

        protected override void SnapHide() {
            CanvasGroup.alpha = 0;
        }

        protected override void OnShow() {
            CanvasGroup.DOFade(1, FadeDuration);
        }

        protected override void OnHide() {
            CanvasGroup.DOFade(0, FadeDuration);
        }

        public Vector2 ViewportMin => new Vector2(.5F - ViewportPos.x / 2, .5F - ViewportPos.y / 2);
        public Vector2 ViewportMax => new Vector2(.5F + ViewportPos.x / 2, .5F + ViewportPos.y / 2);

        private void OnDrawGizmos() {
            var c = Camera.main;
            var a = c.ViewportToWorldPoint(ViewportMin);
            var b = c.ViewportToWorldPoint(ViewportMax);
            Gizmos.DrawLine(a, b);
            if (ToFollow != null) {
                var vpPos = c.WorldToViewportPoint(ToFollow.position);
            }
        }

        private bool isBottom;

        private void Update() {
            if (ToFollow == null) {
                return;
            }

            var c = Camera.main;
            var vpPos = (Vector2) c.WorldToViewportPoint(ToFollow.position);
            var oob = IsOutOfBounds(vpPos);
            Debug.Log($"vpPos = {vpPos} @ {oob}");
            if (!isBottom) {
                transform.position = c.WorldToViewportPoint(ToFollow.position);
            }

            if (!isBottom && oob) {
                GoToBottom();
            }

            if (isBottom && !oob) {
                GoToTop();
            }
        }


        private bool IsOutOfBounds(Vector2 vpPos) {
            return vpPos.x < ViewportPos.x && vpPos.x > 1 - ViewportPos.x && vpPos.y < ViewportPos.y && vpPos.y > 1 - ViewportPos.y;
        }

        private Tween titleAlphaTween;
        private Tween textAlphaTween;
        private Tween sizeTween;
        private Tween titleSizeTween;
        private DialogueTweenState state;

        private enum DialogueTweenState {
            Idle,
            TweeningAlpha,
            TweeningPosition
        }

        private Coroutine tweenCoroutine;

        private void GoToTop() {
            CoroutineUtil.ReplaceCoroutine(ref tweenCoroutine, this, TopRoutine());
        }

        private void GoToBottom() {
            CoroutineUtil.ReplaceCoroutine(ref tweenCoroutine, this, BottomRoutine());
        }

        private IEnumerator BottomRoutine() {
            KillTweens();
            isBottom = true;
            textAlphaTween = TextField.DOFade(0, AlphaTransferDuration);
            state = DialogueTweenState.TweeningAlpha;
            yield return new WaitForSeconds(AlphaTransferDuration);
            var rectTransform = transform as RectTransform;
            LabelField.enabled = true;
            rectTransform.DOAnchorMin(Vector2.zero, SizeTransferDuration);
            rectTransform.DOAnchorMax(new Vector2(1, BottomHeight), SizeTransferDuration);
            titleSizeTween = LabelField.DOFontSize(TitleFontSize, SizeTransferDuration);
            var master = (RectTransform) LayoutGroup.transform;
            var d = master.sizeDelta;
            d.x = TextWidth;
            master.DOSizeDelta(d, SizeTransferDuration);
            yield return new WaitForSeconds(SizeTransferDuration);
            textAlphaTween = TextField.DOFade(1, AlphaTransferDuration);
            titleAlphaTween = LabelField.DOFade(1, AlphaTransferDuration);
        }

        private IEnumerator TopRoutine() {
            isBottom = false;
            KillTweens();
            titleAlphaTween = LabelField.DOFade(0, AlphaTransferDuration);
            textAlphaTween = TextField.DOFade(0, AlphaTransferDuration);
            state = DialogueTweenState.TweeningAlpha;
            yield return new WaitForSeconds(AlphaTransferDuration);
            var rectTransform = transform as RectTransform;
            var half = new Vector2(0.5F, 0.5F);
            rectTransform.DOAnchorMin(half, SizeTransferDuration);
            rectTransform.DOAnchorMax(half, SizeTransferDuration);
            var master = (RectTransform) LayoutGroup.transform;
            titleSizeTween = LabelField.DOFontSize(1, SizeTransferDuration);
            var d = new Vector2(LayoutGroup.preferredWidth, LayoutGroup.preferredHeight);
            sizeTween = master.DOSizeDelta(d, SizeTransferDuration);
            state = DialogueTweenState.TweeningPosition;
            yield return new WaitForSeconds(SizeTransferDuration);
            LabelField.enabled = false;
            textAlphaTween = TextField.DOFade(1, AlphaTransferDuration);
            state = DialogueTweenState.Idle;
        }

        private void KillTweens() {
            sizeTween?.Kill();
            titleAlphaTween?.Kill();
            titleSizeTween?.Kill();
            textAlphaTween?.Kill();
        }
    }
}