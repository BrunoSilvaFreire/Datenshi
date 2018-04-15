using Datenshi.Scripts.Misc.Ghosting;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public class EntityMiscController : MonoBehaviour {
        public AudioSource EntityAudioSource;
        public AudioClip[] StepSounds;
        public GhostingContainer Container;

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

        private void SetMinMax(float a, float b) {
            minPitch = Mathf.Min(a, b);
            maxPitch = Mathf.Max(a, b);
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