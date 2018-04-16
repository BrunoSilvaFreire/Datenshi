using System.Collections;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util.Singleton;
using Shiroi.Cutscenes;
using Sirenix.OdinInspector;

namespace Datenshi.Scripts.Missions {
    public class MissionManager : Singleton<MissionManager> {
        public Mission CurrentMission {
            get {
                var tracker = CurrentTracker;
                return tracker == null ? null : tracker.Mission;
            }
        }

        public MissionTracker CurrentTracker {
            get;
            private set;
        }

        public bool StartMission(Mission mission) {
            if (CurrentMission == null) {
                return false;
            }

            CurrentTracker = new MissionTracker(mission);
            StartCoroutine(DoMissionStart(mission, CurrentTracker));
            return true;
        }

        private IEnumerator DoMissionStart(Mission mission, MissionTracker currentTracker) {
            var player = CutscenePlayer.Instance;
            var start = mission.OnStart;
            if (start != null) {
                yield return player.YieldPlay(start);
            }

            currentTracker.InitObjective();
        }

        public bool StopCurrentMission() {
            if (CurrentMission == null) {
                return false;
            }

            return true;
        }
    }
}