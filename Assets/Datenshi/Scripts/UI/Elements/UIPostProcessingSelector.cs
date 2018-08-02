using Datenshi.Scripts.Game;
using Datenshi.Scripts.Graphics;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Elements {
    public class UIPostProcessingSelector : UIDelegateElement<Toggle> {
        private void Start() {
            Delegate.onValueChanged.AddListener(OnChanged);
        }

        private void OnChanged(bool arg0) {
            GraphicsSingleton.Instance.PostProcessVolume.enabled = arg0;
        } 
    }
}