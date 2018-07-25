using UnityEngine;

namespace Datenshi.Scripts.FX {
    [CreateAssetMenu(menuName = "Datenshi/Effects/SpawnPrefab")]
    public class SpawnPrefabEffect : Effect {
        public GameObject Prefab;

        public override void Execute(Vector3 location) {
            Instantiate(Prefab, location, Quaternion.identity);
        }
    }
}