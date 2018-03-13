using Datenshi.Scripts.Util.Singleton;

namespace Datenshi.Scripts.Game {
    public class RuntimeVariables : Singleton<RuntimeVariables> {
        public bool AllowPlayerInput;
        public bool AllowAIInput;
    }
}