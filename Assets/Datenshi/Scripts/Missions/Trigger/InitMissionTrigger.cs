namespace Datenshi.Scripts.Missions.Trigger {
    public class InitMissionTrigger : MissionTrigger {
        public Mode Function;

        public enum Mode {
            Start,
            Awake
        }

        private void Start() {
            if (Function != Mode.Start) {
                return;
            }

            AttempStart();
        }

        private void Awake() {
            if (Function != Mode.Awake) {
                return;
            }

            AttempStart();
        }
    }
}