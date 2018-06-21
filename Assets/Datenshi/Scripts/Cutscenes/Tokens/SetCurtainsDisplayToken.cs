﻿using System.Collections;
using Datenshi.Scripts.UI.Misc;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class SetCurtainsDisplayToken : Token {
        public bool Reveal;
        public float WaitDuration = 1;
        public override IEnumerator Execute(CutscenePlayer player) {
            var curtains = UICurtainsView.Instance;
            if (!curtains) {
                yield break;
            }
            if (Reveal) {
                curtains.Reveal();
            } else {
                curtains.Conceal();
            }

            yield return new WaitForSeconds(WaitDuration);
        }
    }
}