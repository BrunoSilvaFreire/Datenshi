using Datenshi.Scripts.Character;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Dialogue {
    public class UIDialoguePortrait : UIElement {
        public Image Image;
        public float VerticalOffset;
        public AppearanceMode Last;
        public bool SpriteFacesLeft;
        public Character.Character Character;
        public void Appear(AppearanceMode mode) {
            Last = mode;
            Showing = true;
        }

        private Vector2 GetAppearPos(AppearanceMode mode) {
            var rect = Image.rectTransform;
            var width = rect.sizeDelta.x;
            var offset = mode.Offset;
            width *= Mathf.Abs(rect.localScale.x);
            if (mode.Left) {
                return new Vector2(width / 2 + offset, VerticalOffset);
            } else {
                return new Vector2(-width / 2 - offset, VerticalOffset);
            }
        }

        private Vector2 GetHidePos(AppearanceMode mode) {
            var rect = Image.rectTransform;
            var pos = rect.sizeDelta.x;
            pos *= Mathf.Abs(rect.localScale.x);
            if (mode.Left) {
                pos *= -1;
            }

            return new Vector2(pos / 2, VerticalOffset);
        }

        protected override void SnapShow() {
            UpdateAnchor();
            Image.rectTransform.anchoredPosition = GetAppearPos(Last);
        }


        protected override void SnapHide() {
            UpdateAnchor();
            Image.rectTransform.anchoredPosition = GetHidePos(Last);
        }

        protected override void OnShow() {
            UpdateAnchor();
            Image.rectTransform.DOAnchorPos(GetAppearPos(Last), Last.Duration);
        }

        protected override void OnHide() {
            UpdateAnchor();
            Image.rectTransform.DOAnchorPos(GetHidePos(Last), Last.Duration);
        }


        private void UpdateAnchor() {
            var pos = new Vector2(Last.Left ? 0 : 1, 0);
            var rect = Image.rectTransform;
            rect.anchorMin = pos;
            rect.anchorMax = pos;
            var scale = rect.localScale;
            var left = Last.Left;
            if (!SpriteFacesLeft) {
                left = !left;
            }

            if (left) {
                if (scale.x < 0) {
                    scale.x *= -1;
                }
            } else {
                if (scale.x > 0) {
                    scale.x *= -1;
                }
            }

            rect.localScale = scale;
        }
    }

}