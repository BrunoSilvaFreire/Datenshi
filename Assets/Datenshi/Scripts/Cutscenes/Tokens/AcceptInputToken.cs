using System.Collections;
using Datenshi.Scripts.Data;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class AcceptInputToken : Token {
        public enum AcceptInputTarget {
            Player,
            AI,
            All
        }

        public bool AcceptInput;
        public AcceptInputTarget Target = AcceptInputTarget.All;

        private bool ShouldModify(AcceptInputTarget target) {
            return Target == target || Target == AcceptInputTarget.All;
        }

        public override IEnumerator Execute(CutscenePlayer player) {
            if (ShouldModify(AcceptInputTarget.AI)) {
                RuntimeResources.Instance.AllowAIInput = AcceptInput;
            }

            if (ShouldModify(AcceptInputTarget.Player)) {
                RuntimeResources.Instance.AllowPlayerInput = AcceptInput;
            }

            yield break;
        }
    }
}