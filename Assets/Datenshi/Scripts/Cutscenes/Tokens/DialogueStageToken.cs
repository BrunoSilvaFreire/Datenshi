using System.Collections;
using Datenshi.Scripts.Cutscenes.Dialogue.UI;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class DialogueStageToken : IToken {
        public bool Show;

        public IEnumerator Execute(CutscenePlayer player) {
            UIMainDialogueStage.Instance.Showing = Show;
            yield break;
        }
    }
}