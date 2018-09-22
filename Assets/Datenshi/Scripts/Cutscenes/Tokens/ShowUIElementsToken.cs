using System.Collections;
using Datenshi.Scripts.UI;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class ShowUIElementsToken : Token {
        public bool Show;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            UIView.SetAllElementsShowing(Show);
            yield break;
        }
    }
}