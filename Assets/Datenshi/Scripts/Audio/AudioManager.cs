using System;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Singleton;
using DG.Tweening;
using UnityEngine;

namespace Datenshi.Scripts.Audio {
    public enum AudioCategory {
        SFX,
        BGM
    }

    public class AudioManager : Singleton<AudioManager> {
        public AudioSource SFXSource;
        public AudioSource BGMSource;
        public AudioLowPassFilter LowPassFilter;
        public AudioHighPassFilter HighPassFilter;

        public void ImpactHighFilter(float value, float cutoff, float duration) {
            HighPassFilter.DOKill();
            HighPassFilter.cutoffFrequency = cutoff;
            HighPassFilter.DOFrequency(value, duration);
        }

        public void ImpactLowFilter(float value, float cutoff, float duration) {
            LowPassFilter.DOKill();
            LowPassFilter.cutoffFrequency = cutoff;
            LowPassFilter.DOFrequency(value, duration);
        }

        public void PlayFX(AudioFX fx) {
            var source = GetSource(fx.Category);
            source.PlayOneShot(fx.Clip, fx.Volume);
        }

        private AudioSource GetSource(AudioCategory fxCategory) {
            switch (fxCategory) {
                case AudioCategory.SFX:
                    return SFXSource;
                case AudioCategory.BGM:
                    return BGMSource;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fxCategory), fxCategory, null);
            }
        }
    }
}