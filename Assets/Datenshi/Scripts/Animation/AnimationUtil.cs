using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Animation {
    public class AnimationUtil : MonoBehaviour {
        public void SpawnPrefab(GameObject prefab) {
            prefab.Clone(transform.position);
        }
    }
}