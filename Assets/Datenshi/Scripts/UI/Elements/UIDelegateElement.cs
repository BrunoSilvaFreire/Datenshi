using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Elements {
    public class UIDelegateElement<T> : UICanvasGroupElement where T : Selectable {
        public T Delegate;

        public override void Select() {
            Delegate.Select();
        }

    }
}