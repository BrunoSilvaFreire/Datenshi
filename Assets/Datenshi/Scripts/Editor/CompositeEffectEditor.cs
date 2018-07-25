using System;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.FX;
using UnityEditor;
using UnityEngine;
using UPM.Editor;

namespace Datenshi.Scripts.Editor {
    [CustomEditor(typeof(CompositeEffect))]
    public class CompositeEffectEditor : UnityEditor.Editor {
        private CompositeEffect effect;
        private CompositeAddContent content;

        private void OnEnable() {
            effect = (CompositeEffect) target;
            content = new CompositeAddContent(effect);
        }

        public override void OnInspectorGUI() {
            GUILayout.BeginHorizontal();
            var found = (Effect) EditorGUILayout.ObjectField("Add Existing Effect", null, typeof(Effect), false);
            if (found != null && !effect.SubEffects.Contains(found)) {
                effect.SubEffects.Add(found);
            }

            if (GUILayout.Button("Add New Effect")) {
                PopupWindow.Show(new Rect(0, 0, 300, 0), content);
            }

            GUILayout.EndHorizontal();

            for (var i = 0; i < effect.SubEffects.Count; i++) {
                var effectSubEffect = effect.SubEffects[i];
                GUILayout.BeginHorizontal();
                effectSubEffect.name = GUILayout.TextField(effectSubEffect.name);
                if (GUILayout.Button("Edit")) {
                    Selection.SetActiveObjectWithContext(effectSubEffect, this);
                }

                if (GUILayout.Button("Delete")) {
                    effect.SubEffects.RemoveAt(i);
                    DestroyImmediate(effectSubEffect, true);
                }

                GUILayout.EndHorizontal();
            }
        }
    }

    public class CompositeAddContent : PopupWindowContent {
        private readonly IList<Type> subEffects =
            UPMAssemblyUtil.GetAllTypesOf<Effect>().Where(type => type != typeof(Effect)).ToList();

        private CompositeEffect effect;

        public CompositeAddContent(CompositeEffect effect) {
            this.effect = effect;
        }

        public override Vector2 GetWindowSize() {
            return new Vector2(300, subEffects.Count() * EditorGUIUtility.singleLineHeight);
        }

        public override void OnGUI(Rect rect) {
            for (var i = 0; i < subEffects.Count; i++) {
                var subEffect = subEffects[i];
                if (!GUI.Button(rect.GetLine((uint) i), subEffect.Name)) {
                    continue;
                }

                var e = (Effect) ScriptableObject.CreateInstance(subEffect);
                e.name = subEffect.Name;
                effect.SubEffects.Add(e);
                AssetDatabase.AddObjectToAsset(e, effect);
                AssetDatabase.SaveAssets();
            }
        }
    }
}