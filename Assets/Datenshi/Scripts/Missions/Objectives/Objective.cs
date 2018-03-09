using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Missions.Objectives {
    public sealed class ObjectiveCompletedEvent : UnityEvent<Objective, Mission, MissionTracker> {
        public static readonly ObjectiveCompletedEvent Instance = new ObjectiveCompletedEvent();
        private ObjectiveCompletedEvent() { }
    }

    public sealed class ObjectiveStartEvent : UnityEvent<Objective, Mission, MissionTracker> {
        public static readonly ObjectiveStartEvent Instance = new ObjectiveStartEvent();
        private ObjectiveStartEvent() { }
    }

    public abstract class Objective : ScriptableObject {
        public abstract void Initialize(Mission mission, MissionManager missionManager, MissionTracker tracker);
        public abstract void Terminate(Mission mission, MissionManager missionManager, MissionTracker tracker);
    }
}