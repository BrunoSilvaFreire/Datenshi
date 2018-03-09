using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    [Serializable]
    public struct Bounds2D {
        [SerializeField]
        public Vector2 Center;

        [SerializeField]
        public Vector2 Size;

        private Bounds2D(Vector2 center, Vector2 size) {
            Center = center;
            Size = size;
        }

        public void Expand(float amount) {
            Expand(new Vector2(amount, amount));
        }

        public void Expand(Vector2 amount) {
            Size += amount;
        }

        public Vector3 Min {
            get {
                return Center - Size / 2;
            }
            set {
                SetMinMax(value, Max);
            }
        }

        public Vector3 Max {
            get {
                return this.Center + Size / 2;
            }
            set {
                SetMinMax(Min, value);
            }
        }

        public void SetMinMax(Vector2 min, Vector2 max) {
            Size = max - min;
            Center = min + Size / 2;
        }

        public static implicit operator Bounds2D(Bounds bounds) {
            return new Bounds2D(bounds.center, bounds.size);
        }

        public static implicit operator Bounds(Bounds2D b) {
            return new Bounds(b.Center, b.Size);
        }

        public override string ToString() {
            return string.Format("Bounds2D(Center: {0}, Size: {1}, Min: {2}, Max: {3})", Center, Size, Min, Max);
        }
#if UNITY_EDITOR
        [ShowInInspector]
        public Collider2D CopyFrom {
            get {
                return null;
            }
            set {
                var bounds = value.bounds;
                Center = bounds.center;
                Size = bounds.size;
            }
        }
#endif
    }
}