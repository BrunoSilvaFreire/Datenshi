using System.Collections;
using Datenshi.Scripts.UI.Dialogue;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class PlayDialogueToken : IToken {
        public Dialogue.Dialogue Dialogue;
        public bool CloseOnFinish;

        public IEnumerator Execute(CutscenePlayer player) {
            var stage = UIDialogueStage.Instance;
            //TODO fix
            //yield return stage.PlayDialogue(Dialogue);
            if (CloseOnFinish) {
                stage.Showing = false;
            }
            yield break;
        }
    }
}