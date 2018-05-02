using System.Collections;
using Datenshi.Scripts.Data;
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
                RuntimeResources.Instance.AllowAIInput = AcceptInput;
            }
            if (Target == AcceptInputTarget.PlayerOnly || Target == AcceptInputTarget.All) {
                RuntimeResources.Instance.AllowPlayerInput = AcceptInput;
            }
            yield break;
        }
    }
}