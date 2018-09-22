using Datenshi.Scripts.Game;
using Datenshi.Scripts.UI;
using Datenshi.Scripts.UI.Misc;
using UnityEngine;

namespace Datenshi.Scripts.Master.World {
    public class UIControlZone : MonoBehaviour {
        public bool ShowBlackBar = true;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.ContainsPlayerEntity()) {
                return;
            }
            if (ShowBlackBar) {
                UIBlackBarView.Instance.Show();
            }

            UIView.SetAllElementsShowing(false);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.ContainsPlayerEntity()) {
                return;
            }
            if (ShowBlackBar) {
                UIBlackBarView.Instance.Hide();
            }

            UIView.SetReleaseAllElementsOverride();
        }
    }
}