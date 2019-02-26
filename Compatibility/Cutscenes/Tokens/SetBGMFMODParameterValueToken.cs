using System.Collections;
using Datenshi.Scripts.Audio;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class SetBGMFMODParameterValueToken : Token {
        public string Parameter;
        public float Value;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var bgm = AudioManager.Instance.BGMSource;
            var instance = bgm.EventInstance;
            if (!instance.isValid()) {
                yield break;
            }

            instance.setParameterValue(Parameter, Value);
        }
    }
}