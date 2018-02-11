using System;
using Datenshi.Scripts.Entity.Blueprints;
using Entitas;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Blueprints {
    [CreateAssetMenu(menuName = "Datenshi/Entities/EntityBlueprint")]
    public class EntityBlueprint : SerializedScriptableObject {
        public string ContextIdentifier;

        [NonSerialized, OdinSerialize]
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        private ComponentBlueprint[] components;

        public ComponentBlueprint[] Components {
            get {
                return components ?? (components = new ComponentBlueprint[0]);
            }
        }

        public void Serialize(IEntity entity) {
            if (entity != null) {
                var allComponents = entity.GetComponents();
                var componentIndices = entity.GetComponentIndices();
                components = new ComponentBlueprint[allComponents.Length];
                for (var i = 0; i < allComponents.Length; i++) {
                    Components[i] = new ComponentBlueprint(
                        componentIndices[i],
                        allComponents[i]
                    );
                }
            } else {
                components = new ComponentBlueprint[0];
            }
        }
    }
}