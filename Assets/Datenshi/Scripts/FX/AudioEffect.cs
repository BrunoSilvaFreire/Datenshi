using Datenshi.Scripts.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.FX {
    [CreateAssetMenu(menuName = "Datenshi/Effects/AudioEffect")]
    public class AudioEffect : Effect {
        public AudioFX AudioFX;

        public bool RandomizePitch;

        [ShowIf(nameof(RandomizePitch))]
        public float MinPitch = .9F;

        [ShowIf(nameof(RandomizePitch))]
        public float MaxPitch = 1.1F;

        public override void Execute(Vector3 location) {
            if (RandomizePitch) {
                AudioManager.Instance.PlayFX(AudioFX, MinPitch, MaxPitch);
            } else {
                AudioManager.Instance.PlayFX(AudioFX);
            }
        }
    }
}