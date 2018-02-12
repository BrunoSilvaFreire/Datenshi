using Entitas;

namespace Datenshi.Scripts.Entities.Blueprints {
    public class ComponentBlueprintException : EntitasException {
        public ComponentBlueprintException(string message, string hint)
            : base(message, hint) { }
    }
}