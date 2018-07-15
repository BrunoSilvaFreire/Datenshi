using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class ExecuteFor : Decorator {
        public SharedFloat Duration;

        private float currentDuration;


        public override bool CanExecute() {
            return currentDuration > 0;
        }

        public override void OnChildExecuted(TaskStatus childStatus) {
            currentDuration -= Time.deltaTime;
        }

        public override void OnEnd() {
            currentDuration = Duration.Value;
        }
    }
}