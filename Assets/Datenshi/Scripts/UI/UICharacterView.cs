using Datenshi.Scripts.Character;
using Datenshi.Scripts.Entities.Components.Player;
using Datenshi.Scripts.Util;
using DG.Tweening;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Datenshi.Scripts.UI {
    public class UICharacterView : UIView {
        public CanvasGroup CanvasGroup;
        public float FadeDuration = 1F;
        public Button Button;
        public Text Text;
        public RawImage Image;
        public UICircle ExternalCircle;
        public UICircle InternalCircle;
        public float PaddingFadeDuration;
        private GameEntity entity;
        private PlayerComponent component;

        public void Setup(PlayerComponent component, GameEntity entity) {
            this.entity = entity;
            this.component = component;
            var character = entity.character.Character;
            var color = character.SignatureColor;
            InternalCircle.SetBaseColor(color);

            var playableCharacter = character as PlayableCharacter;
            if (playableCharacter != null) {
                Image.texture = playableCharacter.CharacterScreen;
                Text.text = playableCharacter.Alias;
            }
        }

        private void Awake() {
            Button.onClick.AddListener(OnClick);
            var go = Image.gameObject;
            go.AddListener(EventTriggerType.PointerEnter, OnButtonSelected);
            go.AddListener(EventTriggerType.PointerExit, OnButtonDeselected);
        }

        private void OnButtonDeselected(BaseEventData arg0) {
            InternalCircle.DOPadding(0, PaddingFadeDuration);
        }

        private void OnButtonSelected(BaseEventData arg0) {
            InternalCircle.DOPadding(ExternalCircle.Padding * 2, PaddingFadeDuration);
        }

        private void OnDisable() {
            Button.onClick.RemoveListener(OnClick);
        }

        private void OnClick() {
            Debug.Log("Current Entity = " + component.CurrentEntity);
            component.CurrentEntity = entity;
            Debug.Log("After Entity = " + component.CurrentEntity);
        }

        public override void Select() {
            EventSystem.current.SetSelectedGameObject(Button.gameObject);
        }

        protected override void SnapShow() {
            CanvasGroup.alpha = 1;
        }

        protected override void SnapHide() {
            CanvasGroup.alpha = 0;
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