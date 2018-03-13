// <auto-generated>
// Rewired Constants
// This list was generated on 3/9/2018 5:23:39 PM
// The list applies to only the Rewired Input Manager from which it was generated.
// If you use a different Rewired Input Manager, you will have to generate a new list.
// If you make changes to the exported items in the Rewired Input Manager, you will
// need to regenerate this list.
// </auto-generated>

namespace Datenshi.Input.Constants {
    public static partial class Actions {
        // Default
        public const int Horizontal = 0;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "YAxis")]
        public const int Vertical = 1;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Jump")]
        public const int Jump = 2;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Walk")]
        public const int Walk = 6;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Attack")]
        public const int Attack = 5;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Dash")]
        public const int Dash = 7;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Planning")]
        public const int Planning = 8;
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
