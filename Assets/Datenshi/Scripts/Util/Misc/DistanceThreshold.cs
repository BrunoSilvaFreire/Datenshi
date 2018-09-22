using System;
using UnityEngine;
using UnityUtilities;

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

        public static bool IsWithin(Vector2 location, Vector2 position, float limit, float multiplier = 1) {
            return Vector2.Distance(location, position) <= limit * multiplier;
        }

        public bool IsWithinMinimum(Vector2 location, Vector2 position, float multiplier = 1) {
            return IsWithin(location, position, Minimum, multiplier);
        }

        public bool IsWithinMaximum(Vector2 location, Vector2 position, float multiplier = 1) {
            return IsWithin(location, position, Maximum, multiplier);
        }

        public bool IsWithinMinimum(Transform transform, Vector2 position, float multiplier = 1) {
            return IsWithinMinimum(transform.position, position, multiplier);
        }

        public bool IsWithinMaximum(Transform transform, Vector2 position, float multiplier = 1) {
            return IsWithinMaximum(transform.position, position, multiplier);
        }

        public void DrawGizmos(Vector2 center) {
            var g = Color.green;
            g.a = .5F;
            var y = Color.yellow;
            y.a = .5F;
            GizmosUtility.DrawWireCircle2D(center, Minimum, g);
            GizmosUtility.DrawWireCircle2D(center, Maximum, y);
        }

        public void DrawGizmos(Vector2 center, AnimationCurve distanceMultiplier) {
            var max = distanceMultiplier.keys.MaxBy(keyframe => keyframe.time).time;
            for (int i = 0; i < max; i++) {
                var y = Color.yellow;
                y.a = .5F;
                GizmosUtility.DrawWireCircle2D(center, Maximum * distanceMultiplier.Evaluate(i), y);
            }
        }
    }
}