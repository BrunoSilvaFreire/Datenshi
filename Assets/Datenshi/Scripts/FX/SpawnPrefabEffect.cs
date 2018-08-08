using UnityEngine;

namespace Datenshi.Scripts.FX {
    [CreateAssetMenu(menuName = BasePath + "/SpawnPrefab")]
    public class SpawnPrefabEffect : Effect {
        public GameObject Prefab;

        public override void Execute(Vector3 location) {
            Instantiate(Prefab, location, Quaternion.identity);
        }
    }
}