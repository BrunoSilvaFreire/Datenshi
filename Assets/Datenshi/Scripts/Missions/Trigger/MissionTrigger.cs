using UnityEngine;

namespace Datenshi.Scripts.Missions.Trigger {
    public abstract class MissionTrigger : MonoBehaviour {
        public Mission Mission;

        public void AttempStart() {
            MissionManager.Instance.StartMission(Mission);
        }
    }
}