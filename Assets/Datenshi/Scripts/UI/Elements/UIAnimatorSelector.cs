using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Elements {
    public class UIAnimatorSelector : UIDelegateElement<Slider> {
        public Text Label;
        public RectTransform HandleDragArea;
        public RuntimeAnimatorController[] Animators;
        public Material[] Materials;

        private void Start() {
            Delegate.minValue = 0;
            var count = Animators.Length;
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
            var p = PlayerController.Instance.CurrentEntity as LivingEntity;
            if (p == null) {
                return;
            }

            var i = (int) arg0;
            var newLayout = Animators[i];
            var newMat = Materials[i];
            Label.text = newLayout.name;
            p.MiscController.MainSpriteRenderer.material = newMat;
            p.AnimatorUpdater.Animator.runtimeAnimatorController = newLayout;
        }
    }
}