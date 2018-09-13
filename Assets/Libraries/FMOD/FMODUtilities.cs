using FMOD;

namespace FMODUnity {
    public static class FMODUtilities {
        public static void PrintIfError(this RESULT result) {
            if (result != RESULT.OK) {
                UnityEngine.Debug.LogError($"FMOD error occourred: {result}");
            }
        }
    }
}