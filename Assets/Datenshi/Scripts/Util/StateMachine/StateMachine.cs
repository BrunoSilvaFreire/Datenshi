namespace Datenshi.Scripts.Util.StateMachine {
    public class StateMachine<S> where S : State<S> {
        public S CurrentState {
            get;
            set;
        }


        public void Execute() {
            CurrentState.OnExecute(this);
        }
    }

    public abstract class State<S> where S : State<S> {
        public abstract void OnExecute(StateMachine<S> stateMachine);
    }
}