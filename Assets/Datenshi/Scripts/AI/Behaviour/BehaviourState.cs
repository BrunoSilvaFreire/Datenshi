using Datenshi.Scripts.Debugging;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    public abstract class BehaviourState : ScriptableObject, IDebugabble {
        public abstract void Execute(AIStateInputProvider provider, Entity entity, DebugInfo info);
        public abstract void DrawGizmos(AIStateInputProvider provider, Entity entity, DebugInfo info);
        public abstract string GetTitle();
    }
}