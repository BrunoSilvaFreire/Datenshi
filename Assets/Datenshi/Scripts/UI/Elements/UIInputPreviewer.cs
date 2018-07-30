using System;
using System.Linq;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Elements {
    public class UIInputPreviewer : MonoBehaviour {
        public Text Prefab;

        private void Start() {
            UpdatePreview();
        }

        public void UpdatePreview() {
            transform.ClearChildren();
            var map = GetMap();
            foreach (var actionElementMap in map.AllMaps) {
                Instantiate(Prefab, transform).text = $"{GetName(actionElementMap)}: {actionElementMap.elementIdentifierName}";
            }
        }

        private string GetName(ActionElementMap actionElementMap) {
            var a = ReInput.mapping.GetAction(actionElementMap.actionId);
            string name;
            switch (actionElementMap.axisContribution) {
                case Pole.Positive:
                    name = a.positiveDescriptiveName;
                    break;
                case Pole.Negative:
                    name = a.negativeDescriptiveName;
                    break;
                default:
                    return "Unknown";
            }

            if (string.IsNullOrEmpty(name)) {
                name = a.descriptiveName;
            }

            return name;
        }

        private ControllerMap GetMap() {
            var maps = PlayerController.Instance.Player.CurrentPlayer.controllers.maps;
            foreach (var map in maps.GetAllMaps(ControllerType.Keyboard)) {
                if (map.enabled) {
                    return map;
                }
            }

            return null;
        }
    }
}