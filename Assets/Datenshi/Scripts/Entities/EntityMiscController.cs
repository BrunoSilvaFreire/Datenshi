using Datenshi.Scripts.Entities.Misc.Ghosting;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Misc.Narrator;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.Entities {
    public class EntityMiscController : MonoBehaviour {
        public CanvasGroup EntityCanvas;
        public Narrator EntityNarrator;
        public AudioSource EntityAudioSource;
        public AudioClip[] StepSounds;
        public Animator Animator;
        public GhostingContainer GhostingContainer;
        public float CanvasFadeDuration = .5F;

        public void ShowCanvas() {
            EntityCanvas.DOKill();
            EntityCanvas.DOFade(1, CanvasFadeDuration);
        }

        public void HideCanvas() {
            EntityCanvas.DOKill();
            EntityCanvas.DOFade(0, CanvasFadeDuration);
        }

        [SerializeField, HideInInspector]
        private float minPitch;

        [SerializeField, HideInInspector]
        private float maxPitch;

        [ShowInInspector]
        public float MinPitch {
            get {
                return minPitch;
            }
            set {
                SetMinMax(value, maxPitch);
            }
        }

        [ShowInInspector]
        public float MaxPitch {
            get {
                return maxPitch;
            }
            set {
                SetMinMax(minPitch, value);
            }
        }

        public SpriteRenderer MainSpriteRenderer;

        private void SetMinMax(float a, float b) {
            minPitch = Mathf.Min(a, b);
            maxPitch = Mathf.Max(a, b);
        }

        public void SetEntityAudioSourcePitch(float pitch) {
            if (EntityAudioSource == null) {
                return;
            }

            EntityAudioSource.pitch = pitch;
        }

        public void SetEntityAudioSourcePitchParameter(string parameterName) {
            if (EntityAudioSource == null || Animator == null) {
                return;
            }

            EntityAudioSource.pitch = Animator.GetFloat(parameterName);
        }

        public void PlayStepSound() {
            if (EntityAudioSource == null) {
                return;
            }

            var randomStep = StepSounds.RandomElement();
            EntityAudioSource.pitch = Random.Range(minPitch, maxPitch);
            EntityAudioSource.PlayOneShot(randomStep);
        }

        public void PlayAudioOneShot(AudioClip clip) {
            if (EntityAudioSource == null) {
                return;
            }


            EntityAudioSource.pitch = 1;
            EntityAudioSource.PlayOneShot(clip);
        }
    }
}