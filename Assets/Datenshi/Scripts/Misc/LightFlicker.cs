using UnityEngine;

namespace Datenshi.Scripts.Misc {
    [ExecuteInEditMode]
    public class LightFlicker : MonoBehaviour {
        public Light Light;
        public float MinFlicker;
        public float MaxFlicker;

        private void Reset() {
            Light = GetComponent<Light>();
        }

        private void Update() {
            UpdatePerlin();
            var l = Light;
            if (l == null) {
                return;
            }

            l.intensity = GetRandomIntensity();
        }

        private void UpdatePerlin() {
            var increment = Time.deltaTime * FlickerSpeed;
            x += increment;
            y -= increment;
        }

        private float GetRandomIntensity() {
            var d = MaxFlicker - MinFlicker;
            var r = Mathf.PerlinNoise(x, y);
            return d + MinFlicker;
        }

        public float FlickerSpeed = 1;
        private float x, y;
    }
}