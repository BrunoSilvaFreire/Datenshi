using System.Collections;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Master.Cutscenes {
    public class SetPlayerAITargetToken : Token {
        public Vector2 Target;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var e = PlayerController.GetOrCreateEntity();
            var m = e as MovableEntity;
            if (m == null) {
                yield break;
            }

            m.AINavigator.SetTarget(Target);
        }
    }
}