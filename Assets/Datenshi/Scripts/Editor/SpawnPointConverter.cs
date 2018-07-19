using Datenshi.Scripts.Entities;
using Datenshi.Scripts.World.Rooms.Game;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Datenshi.Scripts.Editor {
    public static class SpawnPointConverter {
        [MenuItem("Assets/Convert")]
        private static void GenerateProxy() {
            foreach (var obj in SceneManager.GetActiveScene().GetRootGameObjects()) {
                foreach (var entity in obj.GetComponentsInChildren<Entity>()) {
                    Convert(entity);
                }
            }
        }

        private static void Convert(Entity entity) {
            var obj = entity.gameObject;
            var prefab = PrefabUtility.GetPrefabObject(entity);
            if (prefab == null) {
                return;
            }

            var parent = obj.transform.parent;
            Object.DestroyImmediate(obj);
            var newObj = new GameObject($"{prefab.name}_SpawnPoint");
            if (parent != null) {
                newObj.transform.parent = parent;
            }

            var point = newObj.AddComponent<SpawnPoint>();
            Debug.Log("Debug: " + prefab);
            point.Prefab = (Entity) prefab;
        }
    }
}