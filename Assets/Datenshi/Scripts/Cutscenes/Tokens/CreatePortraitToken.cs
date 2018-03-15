using System.Collections;
using Datenshi.Scripts.UI.Dialogue;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Futures;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
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

    public class MovePortraitToken : IToken {
        public Reference<UIDialoguePortrait> Portrait;
        public AppearanceMode AppearanceMode;

        public IEnumerator Execute(CutscenePlayer player) {
            Portrait.Resolve(player).Appear(AppearanceMode);
            yield break;
        }
    }
}