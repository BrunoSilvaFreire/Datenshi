using System.Collections;
using Datenshi.Scripts.Misc;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Master.Cutscenes {
    public class InitializeBossFightToken : Token {
        public ExposedReference<BossFightController> Controller;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            Controller.Resolve(player).Begin();
            yield break;
        }
    }
}