using System;
using System.Collections;
using System.Runtime.InteropServices;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Singleton;
using DG.Tweening;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Datenshi.Scripts.Audio {
    public enum AudioCategory {
        SFX,
        BGM
    }

    public class AudioManager : Singleton<AudioManager> {
        public StudioEventEmitter BGMSource;
        public AudioReverbFilter ReverbFilter;
        public float NormalBGMVolume;
        public float CutsceneBGMVolume;
        public float BGMVolumeTransitionDuration = .25F;


        public void PlayFX(AudioFX fx, float minPitch, float maxPitch) {
            PlayFX(fx, Random.Range(minPitch, maxPitch));
        }

        public void PlayFX(AudioFX fx, float pitch = 1) {
            if (fx == null) {
                Debug.LogWarning("FX must not be null!");
                return;
            }

            switch (fx.Category) {
                case AudioCategory.BGM:
                    BGMSource.Stop();
                    BGMSource.Event = fx.FMODEventName;
                    BGMSource.Play();
                    break;
                case AudioCategory.SFX:
                    PlaySFX(fx, pitch);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlaySFX(AudioFX fx, float pitch) {
            EventInstance instance;
            try {
                instance = RuntimeManager.CreateInstance(fx.FMODEventName);
            } catch (EventNotFoundException e) {
                Debug.LogError($"Error when attempting to play fx {fx.name} \'{fx.FMODEventName}\'");
                Debug.LogException(e, this);
                return;
            }

            instance.setPitch(pitch);
            instance.setVolume(fx.Volume);
            var a = new ATTRIBUTES_3D {
                position = transform.position.ToFMODVector(),
                up = Vector3.up.ToFMODVector(),
                forward = Vector3.right.ToFMODVector()
            };
            instance.set3DAttributes(a);
            instance.start();
            instance.release();
        }


        public void StopBGM() {
            BGMSource.Stop();
        }

        public void RestartBGM() {
            StopBGM();
            BGMSource.Play();
        }

        public void SetBGMAudioVolume(AudioLevel level) {
            SetBGMAudioVolume(level, BGMVolumeTransitionDuration);
        }

        public void SetBGMAudioVolume(AudioLevel level, float duration) {
            BGMSource.DOKill();
            var newVolume = GetBGMVolume(level);
            var instance = BGMSource.EventInstance;
            DOTween.To(() => {
                float volume, finalVolume;
                var r = instance.getVolume(out volume, out finalVolume);
                if (r != RESULT.OK) {
                    Debug.LogWarning($"Error while getting event instance volume: {r}");
                }

                return volume;
            }, value => {
                var r = instance.setVolume(value);
                if (r != RESULT.OK) {
                    Debug.LogWarning($"Error while setting event instance volume to {value}: {r}");
                }
            }, newVolume, duration);
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