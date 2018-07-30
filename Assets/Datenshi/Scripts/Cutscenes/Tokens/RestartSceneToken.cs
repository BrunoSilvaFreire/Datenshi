using System.Collections;
using Datenshi.Scripts.Audio;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine.SceneManagement;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class RestartSceneToken : Token {
        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            AudioManager.Instance.Stop();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            yield break;
        }
    }
}