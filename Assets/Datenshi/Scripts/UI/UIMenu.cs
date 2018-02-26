namespace Datenshi.Scripts.UI {
    public abstract class UIMenu : UIElement {
        private UIView[] views;

        private void Awake() {
            views = GetComponentsInChildren<UIView>();
        }
    }
}