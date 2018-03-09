namespace Datenshi.Scripts.Entities.Motors.State.Ground {
    public class GroundMotorStateMachine : MotorStateMachine<GroundMotorState> {
        public GroundMotorStateMachine() {
            CurrentState = NormalGroundMotorState.Instance;
        }
    }

    public abstract class GroundMotorState : MotorState<GroundMotorState> { }
}