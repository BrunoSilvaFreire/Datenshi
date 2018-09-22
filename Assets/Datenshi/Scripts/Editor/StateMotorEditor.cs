using System;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Entities.Motor.States;
using Shiroi.Cutscenes.Editor.Util;
using UnityEditor;
using UnityEngine;
using UnityUtilities;
using UnityUtilities.Editor;

namespace Datenshi.Scripts.Editor {
    [CustomEditor(typeof(StateMotor))]
    public class StateMotorEditor : UnityEditor.Editor {
        private StateSelectorContent stateSelector;
        private StateMotor motor;

        private void OnEnable() {
            motor = (StateMotor) target;
            stateSelector = new StateSelectorContent(motor);
        }

        public override void OnInspectorGUI() {
            var e = Event.current;
            var states = motor.States;
            var notEmpty = states.Count > 0;
            EditorGUILayout.PrefixLabel((notEmpty ? states.Count.ToString() : "No") + " states found");

            if (notEmpty) {
                var toRemove = new List<MovementState>();
                foreach (var state in states) {
                    EditorGUILayout.BeginHorizontal();
                    state.name = EditorGUILayout.TextField(state.name);
                    if (GUILayout.Button("Delete")) {
                        toRemove.Add(state);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                foreach (var state in toRemove) {
                    DestroyImmediate(state, true);
                    AssetDatabase.SaveAssets();
                }
            }

            if (!GUILayout.Button("State")) {
                return;
            }

            var rect = new Rect(e.mousePosition, stateSelector.GetWindowSize());
            PopupWindow.Show(rect, stateSelector);
        }
    }

    public class StateSelectorContent : PopupWindowContent {
        private const float Width = 200;
        private readonly StateMotor motor;
        private static readonly List<Type> KnownPossibleStates = TypeUtility.GetAllTypesOf<MovementState>().ToList();

        public StateSelectorContent(StateMotor motor) {
            this.motor = motor;
        }

        public override Vector2 GetWindowSize() {
            var height = EditorGUIUtility.singleLineHeight * KnownPossibleStates.Count;
            return new Vector2(Width, height);
        }

        public override void OnGUI(Rect rect) {
            var data = KnownPossibleStates;
            for (var i = 0; i < data.Count; i++) {
                var s = data[i];
                var pos = rect.GetLine((uint) i);
                if (!GUI.Button(pos, s.Name)) {
                    continue;
                }

                var instance = (MovementState) motor.AddToAssetFile(s);
                motor.States.Add(instance);
            }
        }
    }
}