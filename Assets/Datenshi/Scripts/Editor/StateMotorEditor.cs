using System;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Entities.Motor.States;
using Lunari.Tsuki;
using Lunari.Tsuki.Editor;
using Lunari.Tsuki.Editor.Extenders;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Editor {
    [CustomEditor(typeof(StateMotor))]
    public class StateMotorEditor : UnityEditor.Editor {
        private TypeSelectorButton stateSelector;
        private StateMotor motor;

        private void OnEnable() {
            motor = (StateMotor) target;
            stateSelector = TypeSelectorButton.Of<MovementState>(
                new GUIContent("Add new state"), type => {
                    var state = (MovementState) motor.AddToAssetFile(type);
                    motor.States.Add(state);
                }
            );
        }

        public override void OnInspectorGUI() {
            var states = motor.States;
            var notEmpty = states.Count > 0;
            using (new EditorGUILayout.VerticalScope(GUIStyles.box)) {
                using (new EditorGUILayout.HorizontalScope()) {
                    EditorGUILayout.PrefixLabel((notEmpty ? states.Count.ToString() : "No") + " states found");
                    stateSelector.OnInspectorGUI();
                }

                if (!notEmpty) {
                    return;
                }

                var toRemove = new List<MovementState>();
                foreach (var state in states) {
                    EditorGUILayout.BeginHorizontal();
                    if (state != null) {
                        state.name = EditorGUILayout.TextField(state.name);
                    } else {
                        EditorGUILayout.LabelField("Null state");
                    }

                    if (GUILayout.Button("Edit")) {
                        Selection.activeObject = state;
                    }

                    if (GUILayout.Button("Delete")) {
                        toRemove.Add(state);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                foreach (var state in toRemove) {
                    DestroyImmediate(state, true);
                    states.Remove(state);
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }
}