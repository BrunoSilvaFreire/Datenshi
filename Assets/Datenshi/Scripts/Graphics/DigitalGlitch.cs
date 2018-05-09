//
// KinoGlitch - Video glitch effect
//
// Copyright (C) 2015 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using UnityEngine;

namespace Kino {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Kino Image Effects/Digital Glitch")]
    public class DigitalGlitch : MonoBehaviour {
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

        [SerializeField]
        private Shader shader;

        private Material material;
        private Texture2D noiseTexture;
        private RenderTexture trashFrame1;
        private RenderTexture trashFrame2;

        private static Color RandomColor() {
            return new Color(Random.value, Random.value, Random.value, Random.value);
        }

        private void SetUpResources() {
            if (material != null)
                return;

            material = new Material(shader) {hideFlags = HideFlags.DontSave};

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
            if (fcount % 13 == 0)
                Graphics.Blit(source, trashFrame1);
            if (fcount % 73 == 0)
                Graphics.Blit(source, trashFrame2);

            material.SetFloat("_Intensity", intensity);
            material.SetTexture("_NoiseTex", noiseTexture);
            var trashFrame = Random.value > 0.5f ? trashFrame1 : trashFrame2;
            material.SetTexture("_TrashTex", trashFrame);

            Graphics.Blit(source, destination, material);
        }
    }
}