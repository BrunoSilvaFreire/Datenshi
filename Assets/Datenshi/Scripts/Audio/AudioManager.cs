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
        public AudioReverbFilter ReverbFilter;
        public float NormalBGMVolume;
        public float CutsceneBGMVolume;
        public float BGMVolumeTransitionDuration = .25F;

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
            switch (fx.Category) {
                case AudioCategory.SFX:
                    source.PlayOneShot(fx.Clip, fx.Volume);
                    break;
                case AudioCategory.BGM:
                    source.Stop();
                    source.clip = fx.Clip;
                    source.Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        public void StopBGM() {
            BGMSource.Stop();
        }

        public void RestartBGM() {
            BGMSource.Play();
        }

        public void SetBGMAudioVolume(AudioLevel level) {
            SetBGMAudioVolume(level, BGMVolumeTransitionDuration);
        }

        public void SetBGMAudioVolume(AudioLevel level, float duration) {
            BGMSource.DOKill();
            var newVolume = GetBGMVolume(level);
            DOTween.To(() => BGMSource.volume, value => BGMSource.volume = value, newVolume, duration);
        }

        private float GetBGMVolume(AudioLevel level) {
            switch (level) {
                case AudioLevel.Normal:
                    return NormalBGMVolume;
                case AudioLevel.Cutscene:
                    return CutsceneBGMVolume;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }

    public enum AudioLevel {
        Normal,
        Cutscene
    }
}