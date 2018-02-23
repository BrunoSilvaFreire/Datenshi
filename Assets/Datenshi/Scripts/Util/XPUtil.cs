using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class XPUtil {
        public static uint GetRequiredXPForLevel(byte level) {
            return (uint) (100 + Mathf.Pow(level, 2) / 2);
        }
    }
}