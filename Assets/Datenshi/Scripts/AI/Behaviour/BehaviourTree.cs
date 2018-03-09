using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    public abstract class BehaviourState : ScriptableObject {
        public abstract void Execute(AIStateInputProvider provider, MovableEntity entity);
    }
}