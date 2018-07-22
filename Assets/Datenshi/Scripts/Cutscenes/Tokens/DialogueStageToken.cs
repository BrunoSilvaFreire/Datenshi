using System.Collections;
using Datenshi.Scripts.Cutscenes.Dialogue.UI;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class DialogueStageToken : Token {
        public bool Show;
        public bool Snapping;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var s = UIMainDialogueStage.Instance;
            if (Snapping) {
                s.SnapShowing(Show);
            } else {
                s.Showing = Show;
            }

            yield break;
        }
    }
}