using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Entities.Misc;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class SetRenderingAim : Action {
        public bool Rendering;
        public AimRenderer Renderer;

        public override TaskStatus OnUpdate() {
            if (Renderer == null) {
                return TaskStatus.Failure;
            }

            Renderer.Render = Rendering;
            return TaskStatus.Success;
        }
    }
}