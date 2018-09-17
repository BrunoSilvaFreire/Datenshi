using System;
using System.Collections;
using UnityEngine;

namespace Datenshi.Scripts.Movement {
    public static class MovableUtility {
        public static void TranslateNoPhysics(this IDatenshiMovable entity, Vector2 targetPos, float duration) {
            entity.StartCoroutine(DoTranslateNoPhysics(entity, targetPos, duration));
        }

        private static IEnumerator DoTranslateNoPhysics(IDatenshiMovable entity, Vector2 targetPos, float duration) {
            var timeLeft = duration;
            var originalPos = entity.GroundPosition;
            entity.ApplyVelocity = false;
            while (timeLeft >= 0) {
                timeLeft -= Time.deltaTime;
                var pos = (duration - timeLeft) / duration;
                entity.Transform.position = Vector2.Lerp(originalPos, targetPos, pos);
                yield return null;
            }

            entity.ApplyVelocity = true;
        }

        public static int XDirectionTo(float thisX, float otherX) {
            return Math.Sign(otherX - thisX);
        }

        public static int XDirectionTo(Vector2 thisV, Vector2 other) {
            return XDirectionTo(thisV.x, other.x);
        }

        public static int XDirectionTo(this ILocatable locatable, Vector2 other) {
            return locatable.XDirectionTo(other.x);
        }

        private static int XDirectionTo(this ILocatable locatable, float otherX) {
            return XDirectionTo(locatable.Center.x, otherX);
        }

        public static float DistanceTo(this ILocatable locatable, Component component) {
            return locatable.DistanceTo(component.transform.position);
        }
        public static float DistanceTo(this ILocatable locatable, Vector2 position) {
            return Vector2.Distance(locatable.Center, position);
        }
    }
}