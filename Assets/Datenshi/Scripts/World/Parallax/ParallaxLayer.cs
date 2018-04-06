using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.World.Parallax {
    [ExecuteInEditMode]
    public class ParallaxLayer : MonoBehaviour {
        public float SpeedX;
        public float SpeedY;
        public bool MoveInOppositeDirection;

        private Vector3 previousCameraPosition;
        private bool previousMoveParallax;

        [ShowInInspector, ReadOnly]
        private Camera gameCamera;

        [ShowInInspector, ReadOnly]
        private ParallaxOption options;

        public Transform CameraTransform {
            get {
                return gameCamera.transform;
            }
        }
#if UNITY_EDITOR

        [SerializeField, ReadOnly]
        private Vector3 storedPosition;


        [ShowInInspector]
        public void SavePosition() {
            storedPosition = transform.position;
        }

        [ShowInInspector]
        public void RestorePosition() {
            transform.position = storedPosition;
        }

        private void OnValidate() {
            gameCamera = Camera.main;
            options = ParallaxOption.Instance;
        }
#endif


        private void Start() {
            gameCamera = Camera.main;
            options = ParallaxOption.Instance;
        }


        private void Update() {
            if (!gameCamera || !options) {
                return;
            }
            if (options.MoveParallax && !previousMoveParallax)
                previousCameraPosition = CameraTransform.position;

            previousMoveParallax = options.MoveParallax;

            if (!Application.isPlaying && !options.MoveParallax) {
                return;
            }

            var distance = CameraTransform.position - previousCameraPosition;
            var direction = (MoveInOppositeDirection) ? -1f : 1f;
            transform.position += Vector3.Scale(distance, new Vector3(SpeedX, SpeedY)) * direction;

            previousCameraPosition = CameraTransform.position;
        }
    }
}