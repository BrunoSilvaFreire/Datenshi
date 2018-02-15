using Datenshi.Scripts.Util.StateMachine;

namespace Datenshi.Scripts.Entities.Components.Movement {
    public abstract class GroundState : State<GroundState, GroundMovement> {
        public abstract bool AllowInteraction();
    }
}