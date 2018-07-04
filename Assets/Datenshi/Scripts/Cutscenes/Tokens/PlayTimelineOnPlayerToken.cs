using System.Collections;
using Datenshi.Scripts.Game;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine.Playables;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class PlayTimelineOnPlayerToken : Token {
        public PlayableAsset Timeline;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var director = PlayerController.Instance.Director;
            director.Play(Timeline);
            var g = director.playableGraph;
            while (g.IsValid() && g.IsPlaying()) {
                yield return null;
            }
        }
    }
}