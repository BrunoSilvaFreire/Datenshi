using System.Collections;
using Datenshi.Scripts.UI;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class ReleaseUIElementsOverrideToken : Token {
        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            foreach (var uiElement in FindObjectsOfType<UIView>()) {
                uiElement.ReleaseOverride();
            }

            yield break;
        }
    }
}