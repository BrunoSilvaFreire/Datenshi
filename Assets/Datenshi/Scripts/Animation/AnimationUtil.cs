using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Util;
using DG.Tweening;
using UnityEngine;

namespace Datenshi.Scripts.Animation {
    public class AnimationUtil : MonoBehaviour {
        public AudioSource Source;

        public void SpawnPrefab(GameObject prefab) {
            prefab.Clone(transform.position);
        }

        public void PlayAudioOneShot(AudioClip clip) {
            if (Source == null) {
                return;
            }

            Source.PlayOneShot(clip);
        }

        public void PlayAudioFX(AudioFX fx) {
            AudioManager.Instance.PlayFX(fx);
        }

        public void DestroyObject() {
            Destroy(gameObject);
        }

        public void DestroyParent() {
            Destroy(transform.parent.gameObject);
        }

    }
}