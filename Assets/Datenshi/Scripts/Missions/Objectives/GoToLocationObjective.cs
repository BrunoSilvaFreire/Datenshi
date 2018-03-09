using Datenshi.Scripts.Util;
using Datenshi.Scripts.World;
using UnityEngine;

namespace Datenshi.Scripts.Missions.Objectives {
    public class GoToLocationObjective : Objective {
        public Collider2D HitboxPrefab;
        public Vector2 Position;
        public Area Area;
        private bool areaLoaded;
        private Collider2D currentCollider;
        private MissionTracker tracker;

        public override void Initialize(Mission mission, MissionManager missionManager, MissionTracker tracker) {
            AreaChangedEvent.Instance.AddListener(OnAreaChanged);
            this.tracker = tracker;
            areaLoaded = WorldManager.Instance.CurrentArea == Area;
            if (areaLoaded) {
                CreateCollider();
            }
        }

        private void OnTrigger() {
            tracker.AdvanceObjective();
        }

        private void OnAreaChanged(Area old, Area newArea) {
            if (areaLoaded && newArea != Area) {
                CreateCollider();
            } else if (!areaLoaded && newArea == Area) {
                DestroyCollider();
            }
        }

        private void DestroyCollider() {
            currentCollider = HitboxPrefab.Clone(Position);
            currentCollider.AddTriggerListener(OnTrigger);
        }


        private void CreateCollider() {
            currentCollider = HitboxPrefab.Clone(Position);
            currentCollider.RemoveTriggerListener(OnTrigger);
        }

        public override void Terminate(Mission mission, MissionManager missionManager, MissionTracker tracker) {
            if (areaLoaded) {
                DestroyCollider();
            }
        }
    }
}