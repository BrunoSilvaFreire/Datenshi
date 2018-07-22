using System.Collections;
using Datenshi.Scripts.Audio;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class SetBGMVolumeLevelToken : Token {
        public AudioLevel Level;
        public float TransitionDuration = 1;
        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            AudioManager.Instance.SetBGMAudioVolume(Level, TransitionDuration);
            yield break;
        }
    }
}