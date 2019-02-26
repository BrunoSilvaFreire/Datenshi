using Datenshi.Scripts.Movement.Config;
using Datenshi.Scripts.Util.Buffs;
using UnityEngine;

namespace Datenshi.Scripts.Movement {
    public interface IMovable : ILocatable {

        Vector2 Velocity {
            get;
            set;
        }

        MovementConfig MovementConfig {
            get;
            set;
        }

        T GetMovementConfigAs<T>() where T : MovementConfig;

        CollisionStatus CollisionStatus {
            get;
            set;
        }

        Collider2D Hitbox {
            get;
        }

        FloatProperty SpeedMultiplier {
            get;
        }

        Vector2 ExternalForces {
            get;
            set;
        }

        bool ApplyVelocity {
            get;
            set;
        }

        bool TimeScaleIndependent {
            get;
            set;
        }

        float DeltaTime {
            get;
        }
    }
}