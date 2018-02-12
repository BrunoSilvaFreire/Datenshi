using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class TextureUtil {
        public static Texture2D CreateTexture(Color color) {
            return CreateTexture(1, 1, color);
        }

        public static Texture2D CreateTexture(int width, int height, Color color) {
            var pixels = new Color[width * height];
            for (var i = 0; i < pixels.Length; ++i) {
                pixels[i] = color;
            }
            var result = new Texture2D(width, height);
            result.SetPixels(pixels);
            result.Apply();
            return result;
        }
    }
}