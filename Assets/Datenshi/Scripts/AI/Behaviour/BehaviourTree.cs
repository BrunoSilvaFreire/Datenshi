using Datenshi.Input;
using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    public abstract class BehaviourState : ScriptableObject {
        public abstract void Execute(AIStateInputProvider provider, Entity entity);
    }
}