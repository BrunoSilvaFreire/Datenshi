using Datenshi.Scripts.Game;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Elements {
    public class UILayoutSelector : UIDelegateElement<Slider> {
        public Text Label;
        public RectTransform HandleDragArea;
        public UIInputPreviewer Previewer;

        private void Start() {
            Delegate.minValue = 0;
            var count = ReInput.mapping.KeyboardLayouts.Count;
            SetupSlider(count);
            SetupHandleDragArea();
            SetupHandle(count);
        }

        private void SetupSlider(int count) {
            Delegate.maxValue = count - 1;
            Delegate.value = 0;
            Delegate.wholeNumbers = true;
            Delegate.onValueChanged.AddListener(OnChanged);
        }

        private void SetupHandle(int count) {
            var handle = Delegate.handleRect;
            var size = handle.sizeDelta;
            size.x = ((RectTransform) Delegate.transform).sizeDelta.x / count;
            handle.sizeDelta = size;
        }

        private void SetupHandleDragArea() {
            HandleDragArea.pivot = new Vector2(.5F, .5F);
            HandleDragArea.anchorMin = Vector2.zero;
            HandleDragArea.anchorMax = Vector2.one;
            HandleDragArea.anchoredPosition = Vector2.zero;
        }

        private void OnChanged(float arg0) {
            var player = PlayerController.Instance.Player.CurrentPlayer;
            var newLayout = ReInput.mapping.KeyboardLayouts[(int) arg0];
            Label.text = newLayout.descriptiveName;
            foreach (var map in player.controllers.maps.GetAllMaps(ControllerType.Keyboard)) {
                map.enabled = map.layoutId == newLayout.id;
                Debug.Log("Map " + map + "," + map.name + " is now " + map.enabled);
            }

            Previewer.UpdatePreview();
        }
    }
}