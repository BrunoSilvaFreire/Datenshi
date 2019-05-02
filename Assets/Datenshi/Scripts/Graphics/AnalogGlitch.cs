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

using System;
using System.Collections.Generic;
using Shiroi.FX.Services;
using UnityEngine;

namespace Datenshi.Scripts.Graphics {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class AnalogGlitch : SingletonServiceController<AnalogGlitch, GlitchMeta> {
        // Scan line jitter

        [SerializeField, Range(0, 1)]
        private float defaultScanLineJitter;

        public float ScanLineJitter {
            get;
            private set;
        }

        // Vertical jump

        [SerializeField, Range(0, 1)]
        private float defaultVerticalJump;

        public float VerticalJump {
            get;
            private set;
        }

        // Horizontal shake

        [SerializeField, Range(0, 1)]
        private float defaultHorizontalShake;

        public float HorizontalShake {
            get;
            private set;
        }

        // Color drift

        [SerializeField, Range(0, 1)]
        private float defaultColorDrift;

        public float ColorDrift {
            get;
            private set;
        }

        [SerializeField]
        private Shader shader;

        private Material material;

        private float verticalJumpTime;
        private static readonly int Drift = Shader.PropertyToID("_ColorDrift");
        private static readonly int Shake = Shader.PropertyToID("_HorizontalShake");
        private static readonly int Jump = Shader.PropertyToID("_VerticalJump");
        private static readonly int LineJitter = Shader.PropertyToID("_ScanLineJitter");

        private void Set(GlitchMeta.GlitchInfo info) {
            ColorDrift = info.ColorDrift;
            HorizontalShake = info.HorizontalLine;
            VerticalJump = info.VerticalJump;
            ScanLineJitter = info.ScanLineJitter;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (material == null) {
                material = new Material(shader) {hideFlags = HideFlags.DontSave};
            }

            verticalJumpTime += Time.deltaTime * VerticalJump * 11.3f;

            var slThresh = Mathf.Clamp01(1.0f - ScanLineJitter * 1.2f);
            var slDisp = 0.002f + Mathf.Pow(ScanLineJitter, 3) * 0.05f;
            material.SetVector(LineJitter, new Vector2(slDisp, slThresh));

            var vj = new Vector2(VerticalJump, verticalJumpTime);
            material.SetVector(Jump, vj);

            material.SetFloat(Shake, HorizontalShake * 0.2f);

            var cd = new Vector2(ColorDrift * 0.04f, Time.time * 606.11f);
            material.SetVector(Drift, cd);

            UnityEngine.Graphics.Blit(source, destination, material);
        }

        protected override void UpdateGameToDefault() {
            ColorDrift = defaultColorDrift;
            HorizontalShake = defaultHorizontalShake;
            VerticalJump = defaultVerticalJump;
            ScanLineJitter = defaultScanLineJitter;
        }

        protected override void UpdateGameTo(IEnumerable<WeightnedMeta<GlitchMeta>> activeMetas) {
            var info = new GlitchMeta.GlitchInfo();
            foreach (var weightnedMeta in activeMetas) {
                var w = weightnedMeta.Weight;
                var i = weightnedMeta.Meta.Evaluate();
                info.ScanLineJitter += w * i.ScanLineJitter;
                info.ColorDrift += w * i.ColorDrift;
                info.HorizontalLine += w * i.HorizontalLine;
                info.VerticalJump += w * i.VerticalJump;
            }

            Set(info);
        }


        protected override void UpdateGameTo(GlitchMeta meta) {
            Set(meta.Evaluate());
        }
    }

    public class GlitchMeta : IComparable<GlitchMeta>, ITimedServiceTickable {
        public GlitchMeta(AnimationCurve scanLineJitter, AnimationCurve verticalJump, AnimationCurve horizontalLine,
            AnimationCurve colorDrift) {
            ScanLineJitter = scanLineJitter;
            VerticalJump = verticalJump;
            HorizontalLine = horizontalLine;
            ColorDrift = colorDrift;
        }

        public AnimationCurve ScanLineJitter {
            get;
        }

        public AnimationCurve VerticalJump {
            get;
        }

        public AnimationCurve HorizontalLine {
            get;
        }

        public AnimationCurve ColorDrift {
            get;
        }

        public struct GlitchInfo {
            public float ScanLineJitter, VerticalJump, HorizontalLine, ColorDrift;

            public GlitchInfo(float scanLineJitter, float verticalJump, float horizontalLine, float colorDrift) {
                ScanLineJitter = scanLineJitter;
                VerticalJump = verticalJump;
                HorizontalLine = horizontalLine;
                ColorDrift = colorDrift;
            }
        }

        public GlitchInfo Evaluate() {
            return new GlitchInfo(
                ScanLineJitter.Evaluate(position),
                VerticalJump.Evaluate(position),
                HorizontalLine.Evaluate(position),
                ColorDrift.Evaluate(position)
            );
        }

        public int CompareTo(GlitchMeta other) {
            return 0;
        }

        private float position;

        public void Tick(ITimedService service) {
            position = service.PercentageCompleted;
        }
    }
}