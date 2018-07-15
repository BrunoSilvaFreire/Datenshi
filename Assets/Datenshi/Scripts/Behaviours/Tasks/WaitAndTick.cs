using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class WaitAndTick : Conditional {
        public SharedFloat TimeLeft;

        public override TaskStatus OnUpdate() {
            return (TimeLeft.Value -= Time.deltaTime) > 0 ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}