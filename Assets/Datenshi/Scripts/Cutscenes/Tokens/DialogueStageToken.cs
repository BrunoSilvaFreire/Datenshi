using System.Collections;
using Datenshi.Scripts.UI.Dialogue;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class DialogueStageToken : IToken {
        public bool Show;

        public IEnumerator Execute(CutscenePlayer player) {
            UIDialogueStage.Instance.Showing = Show;
            yield break;
        }
    }
}