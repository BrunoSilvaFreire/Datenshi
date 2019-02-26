using System.Collections;
using Datenshi.Scripts.UI.Misc;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class FadeCurtainsToken : Token {
        public bool Reveal;
        public float Duration;
        public bool Wait;
        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var curtains = UICurtainsView.Instance;
            if (!curtains) {
                yield break;
            }

            curtains.FadeDuration = Duration;
            if (Reveal) {
                curtains.Reveal();
            } else {
                curtains.Conceal();
            }

            if (Wait) {
                yield return new WaitForSeconds(Duration);
            }
        }
    }
}