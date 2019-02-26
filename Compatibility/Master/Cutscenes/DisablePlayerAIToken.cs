using System.Collections;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Master.Cutscenes {
    public class DisablePlayerAIToken : Token {
        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var m = PlayerController.GetOrCreateEntity<MovableEntity>();
            if (m == null) {
                yield break;
            }

            m.AutoDrive = false;
            m.ReleaseOverrideInputProvider();
        }
    }
}