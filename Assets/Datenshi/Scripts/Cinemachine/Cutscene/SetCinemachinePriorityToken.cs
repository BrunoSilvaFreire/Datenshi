using System.Collections;
using Cinemachine;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cinemachine.Cutscene {
    public class SetCinemachinePriorityToken : Token {
        public ExposedReference<CinemachineVirtualCameraBase> Camera;
        public int Priority;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var c = Camera.Resolve(player);
            if (c == null) {
                yield break;                
            }

            c.Priority = Priority;
        }
    }
}