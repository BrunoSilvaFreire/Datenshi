namespace Datenshi.Scripts.UI {
    public class UIMenu : UIElement {
        private UIView[] views;

        

        private void Awake() {
            views = GetComponentsInChildren<UIView>();
        }

        protected override void OnShow() {
        }

        protected override void OnHide() {
        }
    }
}