using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;

namespace Datenshi.Scripts.UI {
    public class UIPlayerMenu : UIMenu {
        public DatenshiInputProvider InputProvider;

        private void Start() {
            Hide();
            GamePausedChangeEvent.Instance.AddListener(OnPausedChanged);
        }

        private void OnPausedChanged(bool arg0) {
            Showing = arg0;
        }
    }
}