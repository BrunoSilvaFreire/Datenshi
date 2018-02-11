using Entitas;

namespace Datenshi.Scripts.Entity.Blueprints {
    public class ComponentBlueprintException : EntitasException {
        public ComponentBlueprintException(string message, string hint)
            : base(message, hint) { }
    }
}