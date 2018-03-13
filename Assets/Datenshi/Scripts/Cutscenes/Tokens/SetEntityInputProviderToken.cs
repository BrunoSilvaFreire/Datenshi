using System.Collections;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class SetEntityInputProviderToken : IToken {
        public Reference<Entity> Entity;
        public Reference<InputProvider> InputProvider;

        public IEnumerator Execute(CutscenePlayer player) {
            var entity = Entity.Resolve(player);
            entity.InputProvider = InputProvider.Resolve(player);
            yield break;
        }
    }
}