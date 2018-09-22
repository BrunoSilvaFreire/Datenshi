using UnityEngine;

namespace Datenshi.Scripts.Entities.Motor {
    public abstract class Motor : ScriptableObject {
        public abstract void Move(MovableEntity entity);
    }
}