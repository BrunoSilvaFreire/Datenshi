using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Input {
    public class UIInputDisplay : UIView {
        public CanvasGroup Group;
        public float ShowDuration = .1F;
        public float HideDuration = .1F;
        public float ShowAmount = 1;
        public float HideAmount = 0;
        public Text TextField;
        public Image Image;

        private string text;

        private Sprite sprite;

        [ShowInInspector]
        public string Text {
            get {
                return text;
            }
            set {
                text = value;
                TextField.text = value;
                TextField.enabled = true;
                Image.enabled = false;
            }
        }

        [ShowInInspector]
        public Sprite Sprite {
            get {
                return sprite;
            }
            set {
                sprite = value;
                Image.sprite = value;
                Image.enabled = true;
                TextField.enabled = false;
            }
        }

        protected override void SnapShow() {
            Group.alpha = ShowAmount;
        }

        protected override void SnapHide() {
            Group.alpha = HideAmount;
        }

        protected override void OnShow() {
            Group.DOFade(ShowAmount, ShowDuration);
        }

        protected override void OnHide() {
            Group.DOFade(HideAmount, HideDuration);
        }
    }
}