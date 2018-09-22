using System.Collections;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class PlayEffectToken : Token {
        public Effect Effect;
        public Vector3 Location;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            Effect.Play(new EffectContext(player, new PositionFeature(Location)));
            yield break;
        }
    }
}