using UnityEngine;

namespace Datenshi.Scripts.Entities.Motors {
    public class GroundMotorConfig : MotorConfig {
        public const float DefaultDistance = 2;
        public const float DefaultDuration = 0.25F;
        public const float DefaultDashcooldown = 3;
        public const float DefaultMaxAngle = 60;
        public const float DefaultSlopeGroundCheckLength = 0.1F;
        public float DashDuration = DefaultDuration;
        public float DashDistance = DefaultDistance;
        public float DashCooldown = DefaultDashcooldown;
        public float MaxAngle = DefaultMaxAngle;
        public float SlopeGroundCheckLength = DefaultSlopeGroundCheckLength;
        public AnimationCurve SlopeSpeedMultiplier = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve AirControlSlope = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public float WallClimbGravityScale = 0.1F;
        public float WallClimbCounterForce = 1;
        public float JumpCutGravityModifier = 1;
    }
}