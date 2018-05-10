﻿using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Tutorial;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class Follow : Action {
        public AINavigator Navigator;
        public SharedLivingEntity Target;
        public MovableEntity Entity;
        public Transform Override;
        public float TeleportThreshold = 8;

        public override void OnStart() {
            var b = UITutorialBox.Instance;
            b.OnShowTutorial.AddListener(OnShow);
            b.OnHideTutorial.AddListener(OnHide);
        }

        private void OnHide(TutorialTrigger arg0) {
            Override = null;
        }

        private void OnShow(TutorialTrigger arg0) {
            if (arg0.HasCustomLocation) {
                Override = arg0.CustomLocation;
            }
        }

        public override TaskStatus OnUpdate() {
            var t = Target.Value;
            if (t == null) {
                return TaskStatus.Failure;
            }

            var targetPos = Navigator.GetFavourablePosition(Override != null ? (Vector2) Override.position : t.Center);
            if (Vector2.Distance(Entity.Center, targetPos) > TeleportThreshold) {
                Entity.transform.position = targetPos;
                return TaskStatus.Running;
            }

            Navigator.SetTarget(targetPos);
            Navigator.Execute(Entity, (DummyInputProvider) Entity.InputProvider);

            return TaskStatus.Running;
        }
    }
}