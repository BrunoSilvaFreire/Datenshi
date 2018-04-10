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

        private void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
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

        [ShowIf("IsInt")]
        public int IntValue;

        [ShowIf("IsBoolean")]
        public bool BoolValue;

        [ShowIf("IsFloat")]
        public float FloatValue;

        public bool IsInt {
            get {
                return Type == AnimatorControllerParameterType.Int;
            }
        }

        public bool IsFloat {
            get {
                return Type == AnimatorControllerParameterType.Float;
            }
        }

        public bool IsTrigger {
            get {
                return Type == AnimatorControllerParameterType.Trigger;
            }
        }

        public bool IsBoolean {
            get {
                return Type == AnimatorControllerParameterType.Bool;
            }
        }
    }
}