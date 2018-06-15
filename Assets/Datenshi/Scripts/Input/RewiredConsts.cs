// <auto-generated>
// Rewired Constants
// This list was generated on 4/3/2018 7:19:14 PM
// The list applies to only the Rewired Input Manager from which it was generated.
// If you use a different Rewired Input Manager, you will have to generate a new list.
// If you make changes to the exported items in the Rewired Input Manager, you will
// need to regenerate this list.
// </auto-generated>

using System;
using Datenshi.Scripts.Util;
namespace Datenshi.Scripts.Input {
    public enum Actions : int{
        // Default
         Horizontal = 0,
         Vertical = 1,
         Jump = 2,
         Attack = 5,
         Dash = 7,
         Defend = 8,
         Submit = 9,
         Cancel = 10,
         UIHorizontal = 11,
         UIVertical = 12,
         Flip = 15
    }

    public static class ActionsExtensions {
        public static int GetMask(this Actions action) {
            return 1 << (int) action;
        }

        public static readonly Actions[] GamePlayActions = {
            Actions.Horizontal, 
            Actions.Vertical,
            Actions.Jump,
            Actions.Attack,
            Actions.Defend,
            Actions.Dash
            
        };
        public static Actions GetRandomGameplayAction() {
            return GamePlayActions.RandomElement();
        }
    }
    public static partial class Categories {
        public const int Default = 0;
    }
    public static partial class Layouts {
        public static partial class Joystick {
            public const int Default = 0;
        }
        public static partial class Keyboard {
            public const int Default = 0;
        }
        public static partial class Mouse {
            public const int Default = 0;
        }
        public static partial class CustomController {
            public const int Default = 0;
        }
    }
    public static partial class Players {
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "System")]
        public const int System = 9999999;
        [Rewired.Dev.PlayerIdFieldInfo(friendlyName = "Player1")]
        public const int Player1 = 0;
    }
    public static partial class CustomController {
    }
}
