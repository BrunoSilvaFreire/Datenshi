using Datenshi.Scripts.Audio;
using Shiroi.FX.Effects;
using Shiroi.FX.Effects.Requirements;
using Shiroi.FX.Utilities;

namespace Datenshi.Scripts.FX {
    [Icon(Icons.AudioIcon)]
    public class FMODAudioEffect : Effect {
        public AudioFX AudioFX;
        public Range Pitch;

        public override void Play(EffectContext context) {
            AudioManager.Instance.PlayFX(AudioFX, Pitch.Start, Pitch.End);
        }
    }
}