using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.Graphics {
    public class DigitalUIGlitch : MonoBehaviour {
        [SerializeField, Range(0, 1)]
        private float intensity = 0;

        public float Intensity {
            get {
                return intensity;
            }
            set {
                intensity = value;
            }
        }

        public Graphic Graphic;
        private Texture2D noiseTexture;
        private RenderTexture trashFrame1;
        private RenderTexture trashFrame2;

        private static Color RandomColor() {
            return new Color(Random.value, Random.value, Random.value, Random.value);
        }

        private void SetUpResources() {
            noiseTexture = new Texture2D(64, 32, TextureFormat.ARGB32, false) {
                hideFlags = HideFlags.DontSave,
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Point
            };

            trashFrame1 = new RenderTexture(Screen.width, Screen.height, 0);
            trashFrame2 = new RenderTexture(Screen.width, Screen.height, 0);
            trashFrame1.hideFlags = HideFlags.DontSave;
            trashFrame2.hideFlags = HideFlags.DontSave;

            UpdateNoiseTexture();
        }

        private void UpdateNoiseTexture() {
            var color = RandomColor();

            for (var y = 0; y < noiseTexture.height; y++) {
                for (var x = 0; x < noiseTexture.width; x++) {
                    if (Random.value > 0.89f)
                        color = RandomColor();
                    noiseTexture.SetPixel(x, y, color);
                }
            }

            noiseTexture.Apply();
        }

        private void Update() {
            if (Random.value > Mathf.Lerp(0.9f, 0.5f, intensity)) {
                SetUpResources();
                UpdateNoiseTexture();
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            SetUpResources();

            // Update trash frames on a constant interval.
            var fcount = Time.frameCount;
            if (fcount % 13 == 0) {
                UnityEngine.Graphics.Blit(source, trashFrame1);
            }

            if (fcount % 73 == 0) {
                UnityEngine.Graphics.Blit(source, trashFrame2);
            }

            var material = Graphic.material;
            material.SetFloat("_Intensity", intensity);
            material.SetTexture("_NoiseTex", noiseTexture);
            var trashFrame = Random.value > 0.5f ? trashFrame1 : trashFrame2;
            material.SetTexture("_TrashTex", trashFrame);
            Graphic.SetAllDirty();
            UnityEngine.Graphics.Blit(source, destination, material);
        }
    }
}