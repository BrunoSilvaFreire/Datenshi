using Datenshi.Scripts.Entities.Components.Player;
using Datenshi.Scripts.Util;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Datenshi.Scripts.UI.Menus {
    public class UICharacterSelectionMenu : UIMenu {
        public float ToggleDuration;
        public RadialLayout Layout;
        public CanvasGroup Group;
        public UICircle Circle;
        public float OpenRadius = 150F;

        public void AddCharacter(PlayerComponent component, GameEntity character) {
            Instantiate(UIResources.Instance.CharacterViewPrefab, transform).Setup(component, character);
        }

        protected override void OnShow() {
            Group.interactable = true;
            Circle.rectTransform.DOSizeDelta(new Vector2(OpenRadius, OpenRadius), ToggleDuration);
            Group.DOFade(1, ToggleDuration);
            Layout.DORadius(OpenRadius, ToggleDuration);
        }

        protected override void OnHide() {
            Group.interactable = false;
            Circle.rectTransform.DOSizeDelta(Vector2.zero, ToggleDuration);
            Group.DOFade(0, ToggleDuration);
            Layout.DORadius(0, ToggleDuration);
        }
    }
}