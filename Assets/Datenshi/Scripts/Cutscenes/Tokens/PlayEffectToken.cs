using System.Collections;
using Datenshi.Scripts.FX;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class PlayEffectToken : Token {
        public Effect Effect;
        public Vector3 Location;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            Effect.Execute(Location);
            yield break;
        }
    }
}