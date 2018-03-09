namespace Datenshi.Scripts.Util {
    public static class RandomUtil {
        public static bool NextBool() {
            return UnityEngine.Random.value > 0.5;
        }
    }
}