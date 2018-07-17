using System.Collections;
using Datenshi.Scripts.World.Rooms.Game;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class ActivateSpawnerToken : Token {
        public ExposedReference<Spawner> Spawner;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var s = Spawner.Resolve(player);
            if (s != null) {
                s.Begin();
            }
            yield break;
        }
    }
}