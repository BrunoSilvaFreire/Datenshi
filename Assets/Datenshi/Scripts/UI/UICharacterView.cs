using Datenshi.Scripts.Character;
using Datenshi.Scripts.Entities.Components.Player;
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
        private GameEntity entity;
        private PlayerComponent component;
        public void Setup(PlayerComponent component, GameEntity entity) {
            this.entity = entity;
            this.component = component;
            var character = entity.character.Character;
            var playableCharacter = character as PlayableCharacter;
            if (playableCharacter != null) {
                Image.texture = playableCharacter.CharacterScreen;
            }
        }

        private void Awake() {
            Button.onClick.AddListener(OnClick);
        }

        private void OnDisable() {
            Button.onClick.RemoveListener(OnClick);
        }

        private void OnClick() {
            component.CurrentEntity = entity;
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