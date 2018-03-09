using Datenshi.Scripts.Missions.Objectives;

namespace Datenshi.Scripts.Missions {
    public class MissionTracker {
        public MissionTracker(Mission mission) {
            Mission = mission;
            CurrentObjective = 0;
        }

        public void AdvanceObjective() {
            CurrentObjective++;
            ObjectiveCompletedEvent.Instance.Invoke(Mission[CurrentObjective], Mission, this);
        }

        public uint CurrentObjective {
            get;
            private set;
        }

        public Mission Mission {
            get;
            private set;
        }

        public void InitObjective() {
            ObjectiveStartEvent.Instance.Invoke(Mission[CurrentObjective], Mission, this);
        }
    }
}