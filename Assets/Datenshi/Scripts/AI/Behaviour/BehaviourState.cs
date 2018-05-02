using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    public abstract class BehaviourState : ScriptableObject {
        public abstract void Execute(AIStateInputProvider provider, INavigable entity);
        public abstract void DrawGizmos(AIStateInputProvider provider, INavigable entity);
        public abstract string GetTitle();
    }
}