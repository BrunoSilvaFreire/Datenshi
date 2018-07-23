using Datenshi.Scripts.Audio;
using UnityEngine;

namespace Datenshi.Scripts.FX {
    [CreateAssetMenu(menuName = "Datenshi/Effects/AudioEffect")]
    public class AudioEffect : Effect {
        public AudioFX AudioFX;

        public override void Execute(Vector3 location) {
            AudioManager.Instance.PlayFX(AudioFX);
        }
    }

}