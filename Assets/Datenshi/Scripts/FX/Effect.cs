using UnityEngine;

namespace Datenshi.Scripts.FX {
    public abstract class Effect : ScriptableObject {
        public abstract void Execute(Vector3 location);
    }
}