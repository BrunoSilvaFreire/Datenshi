using System.Collections.Generic;
using Datenshi.Scripts.Misc;
using UnityEngine;

namespace Datenshi.Scripts.Util.StateMachine {
    public class StateMachine<S, O> where S : State<S, O> {
        public O Owner {
            get;
            private set;
        }

        private Dictionary<Variable, object> variables = new Dictionary<Variable, object>();

        public StateMachine(S initialState, O owner) {
            Owner = owner;
            CurrentState = initialState;
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
            if (CurrentState == null) {
                Debug.LogWarningFormat("State machine @ {0} doesn't have any state set!", Owner);
                return;
            }

            CurrentState.OnExecute(this);
        }
    }

    public abstract class State<S, O> where S : State<S, O> {
        public abstract void OnExecute(StateMachine<S, O> stateMachine);
    }
}