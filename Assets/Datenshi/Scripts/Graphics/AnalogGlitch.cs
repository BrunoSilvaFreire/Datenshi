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

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Datenshi.Scripts.Graphics {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class AnalogGlitch : MonoBehaviour {
        // Scan line jitter

        [SerializeField, Range(0, 1)]
        private float scanLineJitter = 0;

        public float ScanLineJitter {
            get {
                return scanLineJitter;
            }
            set {
                scanLineJitter = value;
            }
        }

        // Vertical jump

        [SerializeField, Range(0, 1)]
        private float verticalJump = 0;

        public float VerticalJump {
            get {
                return verticalJump;
            }
            set {
                verticalJump = value;
            }
        }

        // Horizontal shake

        [SerializeField, Range(0, 1)]
        private float horizontalShake = 0;

        public float HorizontalShake {
            get {
                return horizontalShake;
            }
            set {
                horizontalShake = value;
            }
        }

        // Color drift

        [SerializeField, Range(0, 1)]
        private float colorDrift = 0;

        public float ColorDrift {
            get {
                return colorDrift;
            }
            set {
                colorDrift = value;
            }
        }

        [SerializeField]
        private Shader shader;

        private Material material;

        private float verticalJumpTime;

        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (material == null) {
                material = new Material(shader) {hideFlags = HideFlags.DontSave};
            }

            verticalJumpTime += Time.deltaTime * verticalJump * 11.3f;

            var slThresh = Mathf.Clamp01(1.0f - scanLineJitter * 1.2f);
            var slDisp = 0.002f + Mathf.Pow(scanLineJitter, 3) * 0.05f;
            material.SetVector("_ScanLineJitter", new Vector2(slDisp, slThresh));

            var vj = new Vector2(verticalJump, verticalJumpTime);
            material.SetVector("_VerticalJump", vj);

            material.SetFloat("_HorizontalShake", horizontalShake * 0.2f);

            var cd = new Vector2(colorDrift * 0.04f, Time.time * 606.11f);
            material.SetVector("_ColorDrift", cd);

            UnityEngine.Graphics.Blit(source, destination, material);
        }

        public TweenerCore<float, float, FloatOptions> DOScanLineJitter(float value, float duration) {
            return DOTween.To(() => scanLineJitter, v => scanLineJitter = v, value, duration);
        }

        public TweenerCore<float, float, FloatOptions> DOColorDrift(float value, float duration) {
            return DOTween.To(() => colorDrift, v => colorDrift = v, value, duration);
        }

        public TweenerCore<float, float, FloatOptions> DOHorizontalShake(float value, float duration) {
            return DOTween.To(() => horizontalShake, v => horizontalShake = v, value, duration);
        }
    }
}