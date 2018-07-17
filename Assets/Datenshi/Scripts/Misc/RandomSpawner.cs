using Datenshi.Scripts.Util;
using UnityEngine;
using UPM.Util;

namespace Datenshi.Scripts.Misc {
    public class RandomSpawner : MonoBehaviour {
        public GameObject Prefab;
        public Bounds2D Bounds;

        public void Spawn() {
            var b = Bounds;
            b.Center += (Vector2) transform.position;
            Instantiate(Prefab, GetRandomLoc(b), Quaternion.identity);
        }

        private Vector2 GetRandomLoc(Bounds2D bounds2D) {
            var startPos = bounds2D.Min;
            startPos.x += Random.value * bounds2D.Size.x;
            startPos.y += Random.value * bounds2D.Size.y;
            return startPos;
        }

        private void OnDrawGizmos() {
            var b = Bounds;
            b.Center += (Vector2) transform.position;
            GizmosUtil.DrawBounds2D(b, Color.green);
        }
    }
}