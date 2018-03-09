using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Layout {
    public class ChildLayoutFitter : MonoBehaviour, ILayoutElement {
        public UIBehaviour Behaviour;
        public Vector2 Padding;
#if UNITY_EDITOR
        private void OnValidate() {
            if (!(Behaviour is ILayoutElement)) {
                Behaviour = null;
            }

            LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        }
#endif
        private ILayoutElement Element {
            get {
                return Behaviour as ILayoutElement;
            }
        }

        private float Fetch(Func<ILayoutElement, float> func) {
            var e = Element;
            return e == null ? 0 : func(e);
        }

        public void CalculateLayoutInputHorizontal() { }

        public void CalculateLayoutInputVertical() { }

        public float minWidth {
            get {
                return Fetch(e => e.minWidth + Padding.x);
            }
        }


        public float preferredWidth {
            get {
                return Fetch(e => e.preferredWidth + Padding.x);
            }
        }

        public float flexibleWidth {
            get {
                return Fetch(e => e.flexibleWidth + Padding.x);
            }
        }

        public float minHeight {
            get {
                return Fetch(e => e.minHeight + Padding.y);
            }
        }

        public float preferredHeight {
            get {
                return Fetch(e => e.preferredHeight + Padding.y);
            }
        }

        public float flexibleHeight {
            get {
                return Fetch(e => e.flexibleHeight + Padding.y);
            }
        }

        public int layoutPriority {
            get {
                var e = Element;
                return e == null ? 0 : e.layoutPriority;
            }
        }
    }
}