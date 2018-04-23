using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Animation {
    public class AnimationUtil : MonoBehaviour {
        public void SpawnPrefab(GameObject prefab) {
            prefab.Clone(transform.position);
        }

        public void PlayAudioOneShot(AudioClip clip) {
            GetComponentInChildren<AudioSource>().PlayOneShot(clip);
        }

        public void DestroyObject() {
            Destroy(gameObject);
        }
    }
}