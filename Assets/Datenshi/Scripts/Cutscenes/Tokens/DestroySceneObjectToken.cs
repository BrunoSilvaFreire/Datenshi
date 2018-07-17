using System.Collections;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class DestroySceneObjectToken : Token {
        public ExposedReference<Object> ToDestroy;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var d = ToDestroy.Resolve(player);
            if (d != null) {
                Destroy(d);
            } else {
                Debug.LogWarning("No object found!");
            } 
            yield break;
        }
    }
}