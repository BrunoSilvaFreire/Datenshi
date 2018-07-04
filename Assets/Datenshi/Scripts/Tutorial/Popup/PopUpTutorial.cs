namespace Datenshi.Scripts.Tutorial.Popup {
    public class PopUpTutorial : Tutorial {
        public PopUpTutorialConfig Content;

        protected override void OnStartTutorial() {
            PopUpTutorialDisplay.Instance.AddConfig(Content);
        }

        protected override void OnStopTutorial() {
            PopUpTutorialDisplay.Instance.RemoveConfig(Content);
        }
    }
}