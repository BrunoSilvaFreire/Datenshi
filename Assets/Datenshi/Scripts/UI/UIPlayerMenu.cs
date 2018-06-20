using Datenshi.Scripts.Input;
using Datenshi.Scripts.UI.Misc;

namespace Datenshi.Scripts.UI {
    public class UIPlayerMenu : UIMenu {
        public DatenshiInputProvider InputProvider;

        private void Start() {
            Hide();
        }

        private void Update() {
            if (InputProvider.GetButtonDown((int) Actions.Cancel)) {
                Showing = !Showing;
            }
        }
    }
}