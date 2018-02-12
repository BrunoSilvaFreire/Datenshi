using Entitas;
using Entitas.Unity;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Components {
    [Game]
    public class VelocityComponent : IComponent {
        public Vector2 Velocity;
    }

    [Game]
    public class AnimatedComponent : IComponent {
        public RuntimeAnimatorController Controller;
    }

    [Game]
    public class ViewComponent : IComponent {
        public EntityLink View;
    }

    [Game]
    public class CharacterComponent : IComponent {
        public Character.Character Character;
    }
}