using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Datenshi.Scripts.Misc {
    public class RectTransformVisualizer : UIBehaviour {
        private RectTransform Transform => transform as RectTransform;

        [ShowInInspector]
        public Vector2 Position {
            get {
                var t = Transform;
                return t == null ? Vector2.zero : (Vector2) t.position;
            }
        }

        [ShowInInspector]
        public Vector2 AnchoredPosition {
            get {
                var t = Transform;
                return t == null ? Vector2.zero : t.anchoredPosition;
            }
        }

        [ShowInInspector]
        public Vector2 LocalPosition {
            get {
                var t = Transform;
                return t == null ? Vector2.zero : (Vector2) t.localPosition;
            }
        }

        [ShowInInspector]
        public Vector2 Pivot {
            get {
                var t = Transform;
                return t == null ? Vector2.zero : t.pivot;
            }
        }

        [ShowInInspector]
        public Vector2 AnchorMin {
            get {
                var t = Transform;
                return t == null ? Vector2.zero : t.anchorMin;
            }
        }

        [ShowInInspector]
        public Vector2 AnchorMax {
            get {
                var t = Transform;
                return t == null ? Vector2.zero : t.anchorMax;
            }
        }
    }
}