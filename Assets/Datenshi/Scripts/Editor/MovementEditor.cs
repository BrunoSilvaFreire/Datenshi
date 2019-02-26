using System;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Entities.Motor.States;
using Lunari.Tsuki;
using Lunari.Tsuki.Editor;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Editor {
    [CustomEditor(typeof(MovementState), true)]
    public class MovementEditor : UnityEditor.Editor {
        private MovementListenerSelectorContent stateSelector;
        private MovementState motor;
        private TypeSelectorButton addListenerButton;

        private void OnEnable() {
            motor = (MovementState) target;
            addListenerButton = TypeSelectorButton.Of<MovementListener>(new GUIContent("Add listener"),
                delegate(Type type) { motor.Listeners.Add((MovementListener) motor.AddToAssetFile(type)); });

            stateSelector = new MovementListenerSelectorContent(motor);
        }

        public override void OnInspectorGUI() {
            var states = motor.Listeners ?? (motor.Listeners = new List<MovementListener>());
            var notEmpty = states.Count > 0;
            using (new EditorGUILayout.VerticalScope(Lunari.Tsuki.Editor.GUISkinProperties.box)) {
                using (new EditorGUILayout.HorizontalScope()) {
                    EditorGUILayout.PrefixLabel((notEmpty ? states.Count.ToString() : "No") + " listeners found");
                    addListenerButton.OnInspectorGUI();
                }

                if (notEmpty) {
                    var toRemove = new List<MovementListener>();
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
            }

            using (new EditorGUILayout.VerticalScope(Lunari.Tsuki.Editor.GUISkinProperties.box)) {
                DrawDefaultInspector();
            }
        }
    }

    public class MovementListenerSelectorContent : PopupWindowContent {
        private const float Width = 200;
        private readonly MovementState motor;
        private static readonly List<Type> KnownPossibleStates = TypeUtility.GetAllTypesOf<MovementListener>().ToList();

        public MovementListenerSelectorContent(MovementState motor) {
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

                var instance = (MovementListener) motor.AddToAssetFile(s);
                motor.Listeners.Add(instance);
            }
        }
    }
}