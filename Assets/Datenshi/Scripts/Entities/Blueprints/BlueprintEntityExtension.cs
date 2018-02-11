using Datenshi.Scripts.Entities.Blueprints;
using Entitas;

namespace Datenshi.Scripts.Entity.Blueprints {

    public static class BlueprintEntityExtension {

        /// Adds all components from the blueprint to the entity.
        /// When 'replaceComponents' is set to true entity.ReplaceComponent()
        /// will be used instead of entity.AddComponent().
        public static void ApplyBlueprint(this IEntity entity, EntityBlueprint entityBlueprint,
                                     bool replaceComponents = false) {
            var componentsLength = entityBlueprint.Components.Length;
            for (int i = 0; i < componentsLength; i++) {
                var componentBlueprint = entityBlueprint.Components[i];
                if (replaceComponents) {
                    entity.ReplaceComponent(componentBlueprint.index,
                                     componentBlueprint.CreateComponent(entity));
                } else {
                    entity.AddComponent(componentBlueprint.index,
                                 componentBlueprint.CreateComponent(entity));
                }
            }
        }
    }
}
