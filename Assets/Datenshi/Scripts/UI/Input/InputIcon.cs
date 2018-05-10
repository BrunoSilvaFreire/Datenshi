using System;
using UnityEngine;

namespace Datenshi.Scripts.UI.Input {
    public abstract class InputIcon : ScriptableObject, IComparable<int> {
        public byte ActionId;
        public abstract void Setup(UIInputDisplay display, bool inverted);

        public int CompareTo(int other) {
            return ActionId.CompareTo((byte) other);
        }
    }
}