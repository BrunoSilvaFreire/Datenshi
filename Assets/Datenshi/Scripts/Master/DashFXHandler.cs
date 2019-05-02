using System;
using Cinemachine;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Motor.States.Listeners;
using Datenshi.Scripts.Game;
using FMODUnity;
using Shiroi.FX.Utilities;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

namespace Datenshi.Scripts.Master {
    public class DashFXHandler : MonoBehaviour {
        public float ChangeSpeed = 0.4F;
        public PostProcessVolume Volume;
        public StudioEventEmitter EventEmitter;
        public CinemachineVirtualCamera Camera;
        private CinemachineBasicMultiChannelPerlin perlin;

        private void Update() {
            var e = PlayerController.Instance.CurrentEntity as MovableEntity;
            bool dashing;
            dashing = false;
            if (e != null) {
                dashing = e.GetVariable(DashListener.DashingLastFrame);
            }

            var target = dashing ? 1 : 0;
            Volume.weight = Mathf.Lerp(Volume.weight, target, ChangeSpeed);
            if (EventEmitter != null) {
                var playing = EventEmitter.IsPlaying();
                if (playing && !dashing) {
                    EventEmitter.Stop();
                }

                if (!playing && dashing) {
                    EventEmitter.Play();
                }
            }

            if (perlin != null) {
                perlin.m_AmplitudeGain = Mathf.Lerp(perlin.m_AmplitudeGain, target, ChangeSpeed);
            } else {
                perlin = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
        }
    }
}