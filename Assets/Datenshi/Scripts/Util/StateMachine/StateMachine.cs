using System.Collections.Generic;
using Datenshi.Scripts.Misc;

namespace Datenshi.Scripts.Util.StateMachine {
    public class StateMachine<S, O> where S : State<S, O> {
        public O Owner {
            get;
            private set;
        }

        private Dictionary<Variable, object> variables = new Dictionary<Variable, object>();

        public StateMachine(O owner) {
            Owner = owner;
        }

        public void SetVariable<T>(Variable<T> variable, T value) {
            variables[variable] = value;
        }

        public T GetVariable<T>(Variable<T> variable) {
            if (variables.ContainsKey(variable)) {
                return (T) variables[variable];
            }
            return variable.DefaultValue;
        }

        public S CurrentState {
            get;
            set;
        }


        public void Execute() {
            CurrentState.OnExecute(this);
        }
    }

    public abstract class State<S, O> where S : State<S, O> {
        public abstract void OnExecute(StateMachine<S, O> stateMachine);
    }
}