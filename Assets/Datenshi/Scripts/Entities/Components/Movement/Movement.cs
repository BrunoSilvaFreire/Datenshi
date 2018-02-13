using Datenshi.Scripts.Controller;
using Datenshi.Scripts.Util.StateMachine;
using Entitas;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Components.Movement {
    [Game]
    public class GroundMovement : Movement {
        public float MaxSpeed = Constants.DefaultSpeed;
        public float MaxJumpHeight = Constants.DefaultJumpHeight;
        public AnimationCurve AccelerationCurve = AnimationCurve.EaseInOut(0, 0.5F, 0.5F, 1F);
        public AnimationCurve DeaccelerationCurve = AnimationCurve.EaseInOut(0, 0.5F, 0.5F, 1F);
        public StateMachine<GroundState> StateMachine;
    }

    public abstract class Movement : IComponent {
        public IInputProvider Provider;
    }
}