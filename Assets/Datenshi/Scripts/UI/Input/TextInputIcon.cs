using UnityEngine;

namespace Datenshi.Scripts.UI.Input {
    public class TextInputIcon : InputIcon {
        public string Text;

        public override void Setup(UIInputDisplay display, bool inverted) {
            display.Image.transform.rotation = Quaternion.identity;
            display.Text = Text;
        }
    }
}