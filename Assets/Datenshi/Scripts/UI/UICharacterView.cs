using Datenshi.Scripts.Character;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI {
    public class UICharacterView : UIView {
        public CanvasGroup CanvasGroup;
        public float FadeDuration = 1F;
        public Button Button;
        public RawImage Image;

        public void Setup(PlayableCharacter character) {
            Image.texture = character.CharacterScreen;
        }

        private void Awake() {
        }

        public override void Select() {
            EventSystem.current.SetSelectedGameObject(Button.gameObject);
        }

        protected override void OnShow() {
            CanvasGroup.DOKill();
            CanvasGroup.DOFade(1, FadeDuration);
        }

        protected override void OnHide() {
            CanvasGroup.DOKill();
            CanvasGroup.DOFade(0, FadeDuration);
        }
    }
}