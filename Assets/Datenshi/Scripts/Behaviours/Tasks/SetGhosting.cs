using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Entities.Misc.Ghosting;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class SetGhosting : Action {
        public GhostingContainer Container;
        public bool Ghosting;

        public override TaskStatus OnUpdate() {
            Container.Spawning = Ghosting;
            return TaskStatus.Success;
        }
    }
}