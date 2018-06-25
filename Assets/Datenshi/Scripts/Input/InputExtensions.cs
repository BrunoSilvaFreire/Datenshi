using UPM.Input;

namespace Datenshi.Scripts.Input {
    public static class InputExtensions {

        public static bool GetButtonDownOrDefault(this InputProvider provider, int button) {
            return provider != null && provider.GetButtonDown(button);
        }
    }
}