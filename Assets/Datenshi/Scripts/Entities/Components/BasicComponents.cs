using Entitas;
using Entitas.Unity;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Components {
    [Game]
    public class HealthComponent : IComponent {
        public uint Health = 200;
        public uint MaxHealth = 200;
    }

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
}