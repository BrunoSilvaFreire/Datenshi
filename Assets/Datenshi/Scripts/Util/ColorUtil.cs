using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class ColorUtil {
        public static void SetBrightness(ref Color color, float brightness) {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            color = Color.HSVToRGB(h, s, brightness);
        }
        public static void SetSaturation(ref Color color, float brightness) {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            color = Color.HSVToRGB(h, brightness, v);
        }
    }
}