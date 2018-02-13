using Datenshi.Scripts.Util.StateMachine;

namespace Datenshi.Scripts.Entities.Components.Movement {
    public abstract class GroundState : State<GroundState> {
        public abstract bool AllowInteraction();
    }
}