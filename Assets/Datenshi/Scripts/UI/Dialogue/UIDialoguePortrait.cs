using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Dialogue {
    public class UIDialoguePortrait : UIElement {
        public Character.Character Character;
        public Image Image;
        public float VerticalOffset;
        public AppearanceMode Last;
        public bool SpriteFacesLeft;

        public void Appear(AppearanceMode mode) {
            Last = mode;
            OnShow();
        }

        private Vector2 GetAppearPos(AppearanceMode mode) {
            var rect = Image.rectTransform;
            var pos = rect.sizeDelta.x;
            var offset = mode.Offset;
            pos *= Mathf.Abs(rect.localScale.x);
            if (mode.Left)
                return new Vector2(pos / 2 + offset, VerticalOffset);
            pos *= -1;
            offset *= -1;

            return new Vector2(pos / 2 + offset, VerticalOffset);
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

    [Serializable]
    public struct AppearanceMode {
        public float Duration;
        public float Offset;
        public bool Left;
        public AppearanceMode(float duration, float offset, bool left) {
            Duration = duration;
            Offset = offset;
            Left = left;
        }
#if UNITY_EDITOR

        [ShowInInspector]
        public bool Right {
            get {
                return !Left;
            }
            set {
                Left = !value;
            }
        }
#endif
    }
}