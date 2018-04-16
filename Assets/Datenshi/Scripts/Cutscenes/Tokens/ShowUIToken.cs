using System.Collections;
using Datenshi.Scripts.UI;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public sealed class ShowUIToken : IToken {
        public bool Override;

        [ShowIf("Override")]
        public bool Show;

        public IEnumerator Execute(CutscenePlayer player) {
            foreach (var view in Object.FindObjectsOfType<UIDefaultColoredElement>()) {
                if (Override) {
                    view.Override(Show);
                } else {
                    view.ReleaseOverride();
                }
            }

            yield break;
        }
    }
}