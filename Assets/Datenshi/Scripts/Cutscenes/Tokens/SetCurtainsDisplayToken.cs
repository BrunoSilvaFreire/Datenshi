using System.Collections;
using Datenshi.Scripts.UI.Misc;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class SetCurtainsDisplayToken : Token {
        public bool Reveal;

        public override IEnumerator Execute(CutscenePlayer player) {
            var curtains = UICurtainsElement.Instance;
            if (Reveal) {
                curtains.Reveal();
            } else {
                curtains.Conceal();
            }

            yield return new WaitForSeconds(curtains.FadeDuration);
        }
    }
}