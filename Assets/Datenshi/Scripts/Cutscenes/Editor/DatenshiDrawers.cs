using Shiroi.Cutscenes.Editor.Drawers;
using UnityEditor;

namespace Datenshi.Scripts.Cutscenes.Editor {
    [InitializeOnLoad]
    public static class DatenshiDrawers {
        static DatenshiDrawers() {
            TypeDrawers.RegisterDrawer(new AppearanceModeDrawer());
        }
    }
}