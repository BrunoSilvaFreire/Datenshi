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
using Datenshi.Scripts.Util.Services;
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
        private readonly ServiceHandler<GlitchMeta> serviceHandler = new ServiceHandler<GlitchMeta>();

        private void Update() {
            serviceHandler.Tick();
            var s = serviceHandler.WithGenericHighestPriority() as TimedService<GlitchMeta>;
            if (s == null) {
                SetDefault();
            } else {
                Set(s);
            }
        }

        public TimedService<GlitchMeta> RequesTimedService(float duration, GlitchMeta meta,
            byte priority = Service.DefaultPriority) {
            return serviceHandler.RegisterTimedService(meta, duration, priority);
        }

        private void Set(TimedService<GlitchMeta> service) {
            var meta = service.Metadata;
            var pos = service.Percentage;
            ColorDrift = meta.ColorDrift.Evaluate(pos);
            HorizontalShake = meta.HorizontalLine.Evaluate(pos);
            VerticalJump = meta.VerticalJump.Evaluate(pos);
            ScanLineJitter = meta.ScanLineJitter.Evaluate(pos);
        }

        private void SetDefault() {
            ColorDrift = defaultColorDrift;
            HorizontalShake = defaultHorizontalShake;
            VerticalJump = defaultVerticalJump;
            ScanLineJitter = defaultScanLineJitter;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (material == null) {
                material = new Material(shader) {hideFlags = HideFlags.DontSave};
            }

            verticalJumpTime += Time.deltaTime * VerticalJump * 11.3f;

            var slThresh = Mathf.Clamp01(1.0f - ScanLineJitter * 1.2f);
            var slDisp = 0.002f + Mathf.Pow(ScanLineJitter, 3) * 0.05f;
            material.SetVector("_ScanLineJitter", new Vector2(slDisp, slThresh));

            var vj = new Vector2(VerticalJump, verticalJumpTime);
            material.SetVector("_VerticalJump", vj);

            material.SetFloat("_HorizontalShake", HorizontalShake * 0.2f);

            var cd = new Vector2(ColorDrift * 0.04f, Time.time * 606.11f);
            material.SetVector("_ColorDrift", cd);

            UnityEngine.Graphics.Blit(source, destination, material);
        }
    }

    public class GlitchMeta : IComparable<GlitchMeta> {
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

        public int CompareTo(GlitchMeta other) {
            return 0;
        }
    }
}