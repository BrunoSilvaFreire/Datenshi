using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Datenshi.Scripts.Util {
    public static class ObjectUtil {
        public static IEnumerable<T> FindAll<T>() {
            return SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(rootGameObject => rootGameObject.GetComponentsInChildren<T>());
        }
    }
}