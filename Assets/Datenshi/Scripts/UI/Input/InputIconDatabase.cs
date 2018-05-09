using System;
using System.Collections.Generic;
using Datenshi.Scripts.UI.Misc;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

#endif
namespace Datenshi.Scripts.UI.Input {
    [CreateAssetMenu]
    public class InputIconDatabase : ScriptableObject {
        public List<InputIcon> Icons = new List<InputIcon>();

        public InputIcon GetIconFor(int action) {
            return Icons.FirstOrDefaultComparable(action);
        }
    }

    [Flags]
    public enum DatenshiInput {
        Attack = 1 << 0,
        Defend = 1 << 1,
        Jump = 1 << 2,
        Dash = 1 << 3,
    }

    public abstract class InputIcon : ScriptableObject, IComparable<int> {
        public byte ActionId;
        public abstract void Setup(UIInputDisplay display, bool inverted);

        public int CompareTo(int other) {
            return ActionId.CompareTo((byte) other);
        }
    }
}