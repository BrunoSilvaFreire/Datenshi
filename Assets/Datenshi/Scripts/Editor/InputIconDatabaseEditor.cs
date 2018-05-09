using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.UI.Input;
using Rewired;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Editor {
    [CustomEditor(typeof(InputIconDatabase))]
    public class InputIconDatabaseEditor : UnityEditor.Editor {
        private InputIconDatabase db;

        private void OnEnable() {
            db = (InputIconDatabase) target;
        }

        private static readonly Dictionary<InputIconDatabase, List<InputIcon>> StatesCache = new Dictionary<InputIconDatabase, List<InputIcon>>();

        public static List<InputIcon> GetAllStates(InputIconDatabase motor) {
            if (StatesCache.ContainsKey(motor)) {
                return StatesCache[motor];
            }

            var path = AssetDatabase.GetAssetPath(motor);
            var found = AssetDatabase.LoadAllAssetsAtPath(path).OfType<InputIcon>().ToList();
            StatesCache[motor] = found;
            return found;
        }

        public static void NotifyNewInstance(InputIconDatabase motor, InputIcon instance) {
            var s = GetAllStates(motor);
            if (!s.Contains(instance)) {
                s.Add(instance);
            }
        }

        public static void DeleteInstance(InputIconDatabase motor, InputIcon state) {
            var s = GetAllStates(motor);
            if (s.Contains(state)) {
                s.Remove(state);
            }

            DestroyImmediate(state, true);
        }

        public override void OnInspectorGUI() {
            var e = Event.current;
            var states = GetAllStates(db);
            var notEmpty = states.Count > 0;
            EditorGUILayout.PrefixLabel((notEmpty ? states.Count.ToString() : "No") + " states found");
            Manager = (InputManager) EditorGUILayout.ObjectField("InputManager", Manager, typeof(InputManager), true);
            if (notEmpty) {
                var toRemove = new List<InputIcon>();
                foreach (var state in states) {
                    EditorGUILayout.BeginHorizontal();
                    if (Manager) {
                        var a = Manager.userData.GetActionById(state.ActionId);
                        EditorGUILayout.LabelField(a == null ? "null" : a.name);
                    }

                    state.ActionId = (byte) EditorGUILayout.IntField("Action", state.ActionId, GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    state.name = EditorGUILayout.TextField(state.name, GUILayout.ExpandWidth(true));
                    if (GUILayout.Button("Delete")) {
                        toRemove.Add(state);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                foreach (var state in toRemove) {
                    DeleteInstance(db, state);
                    AssetDatabase.SaveAssets();
                }
            }

            db.Icons = states;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("New Text Input Icon")) {
                AddTextIcon();
            }

            if (GUILayout.Button("New Sprite Input Icon")) {
                AddSpriteIcon();
            }

            EditorGUILayout.EndHorizontal();
        }

        public InputManager Manager {
            get;
            set;
        }

        private void AddTextIcon() {
            AddIcon<TextInputIcon>();
        }

        private void AddSpriteIcon() {
            AddIcon<SpriteInputIcon>();
        }

        private void AddIcon<T>() where T : InputIcon {
            var icon = CreateInstance<T>();
            AssetDatabase.AddObjectToAsset(icon, db);
            AssetDatabase.SaveAssets();
            NotifyNewInstance(db, icon);
        }
    }
}