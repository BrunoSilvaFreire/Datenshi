using Datenshi.Scripts.UI;
using UnityEngine;

namespace Datenshi.Scripts.Stealth {
    public abstract class HackableElement : MonoBehaviour {

        public UIElement UIElement;
        
        public abstract void Hack();
    }
}