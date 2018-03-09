using System.Collections.Generic;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Util;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

namespace Datenshi.Scripts.UI.Menus {
    public class UICharacterSelectionMenu : UIMenu {
        public float ToggleDuration;
        public RadialLayout Layout;
        public CanvasGroup Group;
        public UICircle Circle;
        public float OpenRadius = 150F;
        private readonly List<UICharacterView> knownViews = new List<UICharacterView>();
        public PlayerInputProvider InputProvider;

        public void AddCharacter(Entity component) {
            var view = Instantiate(UIResources.Instance.CharacterViewPrefab, Layout.transform);
            view.Setup(component, InputProvider);
            knownViews.Add(view);
        }

        private void Update() {
            Showing = InputProvider.GetPlanningMenu();
        }

        protected override void SnapShow() {
            Group.interactable = true;
            Group.alpha = 1;
            var diameter = OpenRadius * 2;
            Circle.rectTransform.sizeDelta = new Vector2(diameter, diameter);
            Layout.Radius = OpenRadius;
            Select();
        }

        private void Select() {
            if (knownViews.IsNullOrEmpty()) {
                return;
            }

            EventSystem.current.SetSelectedGameObject(knownViews[0].Button.gameObject);
        }

        protected override void SnapHide() {
            Group.interactable = false;
            Group.alpha = 0;
            Circle.rectTransform.sizeDelta = Vector2.zero;
            Layout.Radius = 0;
        }

        protected override void OnShow() {
            Group.interactable = true;
            var diameter = OpenRadius * 2;
            Circle.rectTransform.DOSizeDelta(new Vector2(diameter, diameter), ToggleDuration);
            Group.DOFade(1, ToggleDuration);
            Layout.DORadius(OpenRadius, ToggleDuration);
            Select();
        }

        protected override void OnHide() {
            Group.interactable = false;
            Circle.rectTransform.DOSizeDelta(Vector2.zero, ToggleDuration);
            Group.DOFade(0, ToggleDuration);
            Layout.DORadius(0, ToggleDuration);
        }
    }
}