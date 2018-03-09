namespace Datenshi.Scripts.Entities.Motors {
    public class GroundMotorConfig : MotorConfig {
        public const float DefaultDistance = 2;
        public const float DefaultDuration = 0.25F;
        public const float DefaultDashcooldown = 3;
        public float DashDuration = DefaultDuration;
        public float DashDistance = DefaultDistance;
        public float DashCooldown = DefaultDashcooldown;
    }
}