using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Util;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class AnimationUtil : MonoBehaviour {

        public void SpawnPrefab(GameObject prefab) {
            prefab.Clone(transform.position);
        }

        public void PlayEffect(Effect effect) {
            effect.Play(new EffectContext(this, new PositionFeature(transform.position)));
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