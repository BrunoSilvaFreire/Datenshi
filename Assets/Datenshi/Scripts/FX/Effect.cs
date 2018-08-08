using UnityEngine;

namespace Datenshi.Scripts.FX {
    public abstract class Effect : ScriptableObject {
        public const string BasePath = "Datenshi/Effects/World";
        public abstract void Execute(Vector3 location);
    }
}