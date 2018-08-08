using UnityEngine;

namespace Datenshi.Scripts.Entities.FX {
    public abstract class EntityEffect : ScriptableObject {
        public const string BasePath = "Datenshi/Effects/EntityFX";
        public abstract void Execute(Entity entity);

    }
}