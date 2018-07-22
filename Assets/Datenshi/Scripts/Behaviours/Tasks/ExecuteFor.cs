using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class ExecuteFor : Decorator {
        public SharedFloat Duration;

        public float currentDuration;


        public override bool CanExecute() {
            return currentDuration > 0;
        }

        public override void OnStart() {
            currentDuration = Duration.Value;
        }


        public override void OnChildExecuted(TaskStatus childStatus) {
            currentDuration -= Time.deltaTime;
        }
    }
}