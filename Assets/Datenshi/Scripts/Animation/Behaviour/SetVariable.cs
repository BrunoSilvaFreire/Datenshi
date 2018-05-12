using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Animation.Behaviour {
    public class SetVariable : StateMachineBehaviour {
        public SetVariableAction[] OnEnter;
        public SetVariableAction[] OnExit;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            foreach (var setVariableAction in OnEnter) {
                Execute(setVariableAction, animator);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            foreach (var setVariableAction in OnExit) {
                Execute(setVariableAction, animator);
            }
        }

        private static void Execute(SetVariableAction action, Animator animator) {
            switch (action.Type) {
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(action.ParameterName, action.FloatValue);
                    break;
                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(action.ParameterName, action.IntValue);
                    break;
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(action.ParameterName, action.BoolValue);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    animator.SetTrigger(action.ParameterName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    [Serializable]
    public struct SetVariableAction {
        public string ParameterName;
        public AnimatorControllerParameterType Type;

        [ShowIf(nameof(IsInt))]
        public int IntValue;

        [ShowIf(nameof(IsBoolean))]
        public bool BoolValue;

        [ShowIf(nameof(IsTrigger))]
        public float FloatValue;

        public bool IsInt => Type == AnimatorControllerParameterType.Int;

        public bool IsFloat => Type == AnimatorControllerParameterType.Float;

        public bool IsTrigger => Type == AnimatorControllerParameterType.Trigger;

        public bool IsBoolean => Type == AnimatorControllerParameterType.Bool;
    }
}