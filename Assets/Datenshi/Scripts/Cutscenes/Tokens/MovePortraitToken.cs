using System.Collections;
using Datenshi.Scripts.Cutscenes.Dialogue;
using Datenshi.Scripts.Cutscenes.Dialogue.UI;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class MovePortraitToken : Token {
        public Reference<UIDialoguePortrait> Portrait;
        public AppearanceMode AppearanceMode;

        public override IEnumerator Execute(CutscenePlayer player) {
            Portrait.Resolve(player).Appear(AppearanceMode);
            yield break;
        }
    }
}