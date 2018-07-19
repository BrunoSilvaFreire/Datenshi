using System;
using UnityEngine;

namespace Datenshi.Scripts.Util.Misc {
    [Serializable]
    public struct DistanceThreshold {
        /// <summary>
        /// The distance 
        /// </summary>
        public float Minimum;

        public float Maximum;

        public DistanceThreshold(float minimum, float maximum) {
            Minimum = minimum;
            Maximum = maximum;
        }

        public static bool IsWithin(Vector2 location, Vector2 position, float limit) {
            return Vector2.Distance(location, position) <= limit;
        }

        public bool IsWithinMinimum(Vector2 location, Vector2 position) {
            return IsWithin(location, position, Minimum);
        }

        public bool IsWithinMaximum(Vector2 location, Vector2 position) {
            return IsWithin(location, position, Maximum);
        }

        public bool IsWithinMinimum(Transform transform, Vector2 position) {
            return IsWithinMinimum(transform.position, position);
        }

        public bool IsWithinMaximum(Transform transform, Vector2 position) {
            return IsWithinMaximum(transform.position, position);
        }

        public void DrawGizmos(Vector2 center) {
            var g = Color.green;
            g.a = .5F;
            var y = Color.yellow;
            y.a = .5F;
            DebugUtil.DrawWireCircle2D(center, Minimum, g);
            DebugUtil.DrawWireCircle2D(center, Maximum, y);
        }
    }
}