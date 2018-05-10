using System.Collections;
using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class CoroutineUtil {
        public static void ReplaceCoroutine(ref Coroutine coroutine, MonoBehaviour owner, IEnumerator newRoutine) {
            coroutine.StopIfNotNull(owner);
            coroutine = owner.StartCoroutine(newRoutine);
        }

        public static void StopIfNotNull(this Coroutine routine, MonoBehaviour owner) {
            routine?.Stop(owner);
        }

        public static void Stop(this Coroutine routine, MonoBehaviour owner) {
            owner.StopCoroutine(routine);
        }
    }
}