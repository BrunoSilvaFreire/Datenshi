using System.Collections;
using Datenshi.Scripts.UI;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class OverrideUIElementsToken : Token {
        public bool Value;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            foreach (var uiElement in FindObjectsOfType<UIView>()) {
                Debug.Log($"Overring {uiElement} to {Value}");
                uiElement.Override(Value);
            }

            yield break;
        }
    }
}