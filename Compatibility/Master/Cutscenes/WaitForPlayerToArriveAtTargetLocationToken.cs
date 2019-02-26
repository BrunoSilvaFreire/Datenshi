using System.Collections;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Movement;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Master.Cutscenes {
    public class WaitForPlayerToArriveAtTargetLocationToken : Token {
        public float Radius = 1;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var e = PlayerController.GetOrCreateEntity<MovableEntity>();
            if (e == null) {
                yield break;
            }

            while (e.DistanceTo(e.AINavigator.GetTarget()) > Radius) {
                yield return null;
            }
        }
    }
}