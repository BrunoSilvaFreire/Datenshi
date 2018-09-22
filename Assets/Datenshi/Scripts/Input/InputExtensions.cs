
namespace Datenshi.Scripts.Input {
    public static class InputExtensions {

        public static bool GetButtonDownOrDefault(this DatenshiInputProvider provider, int button) {
            return provider != null && provider.GetButtonDown(button);
        }
    }
}