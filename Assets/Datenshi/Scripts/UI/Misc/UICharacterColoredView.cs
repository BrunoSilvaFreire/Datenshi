using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.UI.Misc {
    public abstract class UICharacterColoredView : UIDefaultColoredView {
        protected override bool HasColorAvailable() {
            var entity = PlayerController.Instance.CurrentEntity;
            if (entity == null) {
                return false;
            }

            return entity.Character != null;
        }

        protected override Color GetAvailableColor() {
            return PlayerController.Instance.CurrentEntity.Character.SignatureColor;
        }
    }
}