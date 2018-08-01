using System;
using System.Linq;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Input;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.Tutorials.Instructions {
    public class InstructionDisplay : MonoBehaviour, ILayoutElement {
        public Image ImageDisplay;
        public Text TextDisplay;
        public Sprite Sprite;
        public Actions Action;

        [SerializeField]
        public DisplayMode mode;


        public enum DisplayMode {
            Sprite,
            Text
        }

        private void Start() {
            UpdateMode();
            ReInput.ControllerConnectedEvent += OnControllerConnected;
            ReInput.ControllerDisconnectedEvent -= OnControllerDisconnected;
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        private void OnControllerDisconnected(ControllerStatusChangedEventArgs obj) {
            UpdateController();
        }

        private void OnControllerConnected(ControllerStatusChangedEventArgs obj) {
            UpdateController();
        }

        private void UpdateController() {
            var rewiredPlayer = PlayerController.Instance.Player.CurrentPlayer;
            Mode = HasJoystick(rewiredPlayer) ? DisplayMode.Sprite : DisplayMode.Text;
        }

        private static bool HasJoystick(Player rewiredPlayer) {
            return rewiredPlayer.controllers.Controllers.Any(c => c.type == ControllerType.Joystick);
        }

        public DisplayMode Mode {
            get {
                return mode;
            }
            set {
                mode = value;
                UpdateMode();
            }
        }

        private void UpdateMode() {
            switch (mode) {
                case DisplayMode.Sprite:
                    TextDisplay.enabled = false;
                    ImageDisplay.enabled = true;
                    ImageDisplay.sprite = Sprite;
                    break;
                case DisplayMode.Text:
                    TextDisplay.enabled = true;
                    ImageDisplay.enabled = false;
                    TextDisplay.text = GetText();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetText() {
            var rewiredPlayer = PlayerController.Instance.Player.CurrentPlayer;
            if (rewiredPlayer == null) {
                return "No Player";
            }

            foreach (var controllerMap in rewiredPlayer.controllers.maps.GetAllMaps(ControllerType.Keyboard)) {
                if (!controllerMap.enabled) {
                    Debug.Log("Controller map " + controllerMap.name + " is disabled");
                    continue;
                }

                var found = controllerMap.GetFirstButtonMapWithAction((int) Action);
                if (found == null) {
                    Debug.Log("Controller map" + controllerMap.name + " found no result");
                    continue;
                }

                return found.elementIdentifierName;
            }

            return "Unknown";
        }
#if UNITY_EDITOR
        private void OnValidate() {
            UpdateMode();
        }
#endif

        private ILayoutElement GetActiveGraphic() {
            switch (mode) {
                case DisplayMode.Sprite:
                    return ImageDisplay;
                case DisplayMode.Text:
                    return TextDisplay;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public void CalculateLayoutInputHorizontal() {
            GetActiveGraphic().CalculateLayoutInputHorizontal();
        }

        public void CalculateLayoutInputVertical() {
            GetActiveGraphic().CalculateLayoutInputVertical();
        }

        public float minWidth => GetActiveGraphic().minWidth;

        public float preferredWidth => GetActiveGraphic().preferredWidth;

        public float flexibleWidth => GetActiveGraphic().flexibleWidth;

        public float minHeight => GetActiveGraphic().minHeight;

        public float preferredHeight => GetActiveGraphic().preferredHeight;

        public float flexibleHeight => GetActiveGraphic().flexibleHeight;

        public int layoutPriority => GetActiveGraphic().layoutPriority;
    }
}