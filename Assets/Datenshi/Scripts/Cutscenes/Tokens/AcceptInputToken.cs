using System;
using System.Collections;
using Datenshi.Scripts.Game;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class AcceptInputToken : IToken {
        public enum AcceptInputTarget {
            PlayerOnly,
            AIOnly,
            All
        }

        public bool AcceptInput;
        public AcceptInputTarget Target = AcceptInputTarget.All;

        public IEnumerator Execute(CutscenePlayer player) {
            if (Target == AcceptInputTarget.AIOnly || Target == AcceptInputTarget.All) {
                RuntimeVariables.Instance.AllowAIInput = AcceptInput;
            }
            if (Target == AcceptInputTarget.PlayerOnly || Target == AcceptInputTarget.All) {
                RuntimeVariables.Instance.AllowPlayerInput = AcceptInput;
            }
            yield break;
        }
    }
}