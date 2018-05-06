using UPM.Motors.Config;

namespace Datenshi.Scripts.Movement.Config {
    public class DatenshiGroundConfig : GroundMotorConfig {
        public float OffWallTimeMargin = .3f;
        public float WallClimbCounterForce = 10;
        public float RejumpLength = .3F;

        public float LeftGroundFor {
            get;
            set;
        }

        public bool IssuedJumpCall {
            get;
            set;
        }
    }
}