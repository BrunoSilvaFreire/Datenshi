using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Util;
using Lunari.Tsuki;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class SetSceneLinkedTargetLocation : Action {
        public string[] Keys = {"MyLocation"};
        public SharedVector2 Position;
        public SceneLinkerAcessor Acessor;
        public SelectionMode Mode;

        public enum SelectionMode {
            Random,
            Closest
        }

        public override TaskStatus OnUpdate() {
            switch (Mode) {
                case SelectionMode.Random:
                    return GetRandom();
                case SelectionMode.Closest:
                    return GetClosest();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private TaskStatus GetClosest() {
            Vector2? closest = null;
            var distance = float.MaxValue;
            foreach (var key in Keys) {
                var obj = Acessor[key];
                Vector2 pos;
                if (!obj.AttemptRetrievePosition(out pos)) {
                    Debug.LogError($"Found no object with key \'{key}\'");
                    continue;
                }

                var d = Vector2.Distance(pos, transform.position);

                if (d > distance) {
                    continue;
                }

                distance = d;
                closest = pos;
            }

            if (closest == null) {
                Debug.LogError("Found no closest");
                return TaskStatus.Failure;
            }

            Position.Value = closest.Value;
            return TaskStatus.Success;
        }

        private TaskStatus GetRandom() {
            var obj = Acessor[Keys.RandomElement()];
            Vector2 pos;
            if (obj.AttemptRetrievePosition(out pos)) {
                Position.Value = pos;
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}