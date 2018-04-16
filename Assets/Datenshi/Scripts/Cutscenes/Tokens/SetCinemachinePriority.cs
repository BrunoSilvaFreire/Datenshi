using System.Collections;
using Cinemachine;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class SetCinemachinePriority : IToken {
        public Reference<CinemachineVirtualCameraBase> Camera;
        public int Priority;

        public IEnumerator Execute(CutscenePlayer player) {
            Camera.Resolve(player).Priority = Priority;
            yield break;
        }
    }
}