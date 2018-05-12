using System.Collections;
using Datenshi.Scripts.Cutscenes.Dialogue.UI;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class CreatePortraitToken : IToken, IFutureProvider, ITokenChangedListener {
        public Character.Character Character;

        [SerializeField]
        private int future;

        public string FutureName = "future_portrait";

        public IEnumerator Execute(CutscenePlayer player) {
            var portrait = UIDialogueStage.Instance.AddPortrait(Character);
            player.ProvideFuture(portrait, future);
            yield break;
        }

        public void RegisterFutures(Cutscene cutscene) {
            future = cutscene.NotifyFuture<UIDialoguePortrait>(this, FutureName);
        }

        public void OnChanged(Cutscene cutscene) {
            cutscene.FutureManager.GetFuture(future).Name = FutureName;
        }
    }
}