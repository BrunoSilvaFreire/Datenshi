using System.Collections;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class AddAudioToken : IToken {
        public bool DeleteOnFinish;

        public IEnumerator Execute(CutscenePlayer player) {
            yield break;            
        }
    }
}