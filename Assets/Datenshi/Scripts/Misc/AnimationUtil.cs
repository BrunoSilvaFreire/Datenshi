using Datenshi.Scripts.Audio;
using Datenshi.Scripts.FX;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class AnimationUtil : MonoBehaviour {

        public void SpawnPrefab(GameObject prefab) {
            prefab.Clone(transform.position);
        }

        public void PlayEffect(Effect effect) {
            effect.Execute(transform.position);
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