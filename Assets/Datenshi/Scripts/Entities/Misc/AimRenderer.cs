#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Datenshi.Scripts.Entities.Misc {
    [ExecuteInEditMode]
    public class AimRenderer : MonoBehaviour {
        public Entity Entity;
        public bool Render;
        public LineRenderer Renderer;
        public float Distance = 5;
        public float StartOffset = 1;
        public Vector2 CenterOffset;

        private void Awake() {
            Renderer.positionCount = 2;
        }

        private void Update() {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying && Render) {
                UpdateLine(Vector2.left);
                return;
            }
#endif
            Renderer.gameObject.SetActive(Render);
            if (!Render) {
                return;
            }

            var input = Entity.InputProvider;
            if (input == null) {
                return;
            }

            var dir = input.GetInputVector();
            UpdateLine(dir);
        }

        private void UpdateLine(Vector2 dir) {
            var positions = new Vector3[2];
            Renderer.GetPositions(positions);
            var entityPos = Entity.Center + CenterOffset;
            positions[0] = entityPos + dir * StartOffset;
            positions[1] = entityPos + dir * Distance;
            Renderer.SetPositions(positions);
        }
    }
}