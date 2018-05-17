using UnityEngine.UI;

namespace Datenshi.Scripts.Util {
    public static class UIUtil {
        public static void SetAlpha(this Graphic graphic, float alpha) {
            var c = graphic.color;
            c.a = alpha;
            graphic.color = c;
        }
    }
}