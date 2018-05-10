using System;
using System.Collections.Generic;
using Datenshi.Scripts.Util;
using UnityEngine;
#if UNITY_EDITOR

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
}