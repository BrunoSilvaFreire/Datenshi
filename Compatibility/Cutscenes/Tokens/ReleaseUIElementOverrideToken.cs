using System.Collections;
using Datenshi.Scripts.UI;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class ReleaseUIElementOverrideToken : Token {
        public Reference<UIElement> Element;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var uiElement = Element.Resolve(player);
            if (uiElement != null) {
                uiElement.ReleaseOverride();
            }

            yield break;
        }
    }
}