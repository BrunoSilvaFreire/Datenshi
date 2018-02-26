using Datenshi.Scripts.Controller;
using Datenshi.Scripts.Util.StateMachine;
using Entitas;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Components.Movement {
    [Game]
    public class GroundMovementBlueprint : IComponent {
        public float MaxSpeed = Constants.DefaultSpeed;
        public float MaxJumpHeight = Constants.DefaultJumpHeight;
        public float GravityScale = 1F;
        public AnimationCurve AccelerationCurve = AnimationCurve.EaseInOut(0, 0.5F, 0.5F, 1F);
        public AnimationCurve DeaccelerationCurve = AnimationCurve.EaseInOut(0, 0.5F, 0.5F, 1F);
        public Controller2D ControllerPrefab;
    }

    [Game]
    public class GroundMovement : Movement {
        public float MaxSpeed = Constants.DefaultSpeed;
        public float GravityScale = 1F;
        public float MaxJumpHeight = Constants.DefaultJumpHeight;
        public AnimationCurve AccelerationCurve = AnimationCurve.EaseInOut(0, 0.5F, 0.5F, 1F);
        public AnimationCurve DeaccelerationCurve = AnimationCurve.EaseInOut(0, 0.5F, 0.5F, 1F);
        public StateMachine<GroundState, GameEntity> StateMachine;
        public Controller2D Controller;
        public float SpeedMultiplier = 1F;
    }

    public abstract class Movement : IComponent {
        public IInputProvider Provider;
    }
}