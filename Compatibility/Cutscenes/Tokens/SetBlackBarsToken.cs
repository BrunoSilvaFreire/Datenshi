using System.Collections;
using Datenshi.Scripts.UI.Misc;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class SetBlackBarsToken : Token {
        public bool Showing;
        public bool ShowDialogue;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var v = FindObjectOfType<UIBlackBarView>();
            if (v == null) {
                yield break;
            }

            v.Showing = Showing;
            v.ShowDialogue = ShowDialogue;
        }
    }
}