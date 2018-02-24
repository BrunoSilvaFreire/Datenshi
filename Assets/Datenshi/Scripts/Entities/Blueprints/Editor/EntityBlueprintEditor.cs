using System;
using System.Linq;
using DesperateDevs.Unity.Editor;
using DesperateDevs.Utils;
using Entitas;
using Entitas.VisualDebugging.Unity.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Blueprints.Editor {
    [CustomEditor(typeof(EntityBlueprint))]
    public class EntityBlueprintInspector : UnityEditor.Editor {
        public static EntityBlueprint[] FindAllBlueprints() {
            return AssetDatabase.FindAssets("l:" + EntityBlueprintPostprocessor.AssetLabel)
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<EntityBlueprint>(path))
                .ToArray();
        }

        [DidReloadScripts, MenuItem("Tools/Entitas/Blueprints/Update all Blueprints", false, 300)]
        public static void UpdateAllEntityBlueprints() {
            if (!EditorApplication.isPlayingOrWillChangePlaymode) {
                var allContexts = FindAllContexts();
                if (allContexts == null) {
                    return;
                }

                var binaryBlueprints = FindAllBlueprints();
                var allContextNames = allContexts.Select(context => context.contextInfo.name).ToArray();
                var updated = 0;
                foreach (var binaryBlueprint in binaryBlueprints) {
                    var didUpdate = UpdateEntityBlueprint(binaryBlueprint, allContexts, allContextNames);
                    if (didUpdate) {
                        updated += 1;
                    }
                }

                if (updated > 0) {
                    Debug.Log(
                        "Validated " + binaryBlueprints.Length + " Blueprints, " + updated + " have been updated.");
                }
            }
        }

        public static bool UpdateEntityBlueprint(EntityBlueprint binaryBlueprint, IContext[] allContexts,
            string[] allContextNames) {
            var needsUpdate = false;

            var contextIndex = Array.IndexOf(allContextNames, binaryBlueprint.ContextIdentifier);
            if (contextIndex < 0) {
                contextIndex = 0;
                needsUpdate = true;
            }

            var context = allContexts[contextIndex];
            binaryBlueprint.ContextIdentifier = context.contextInfo.name;

            foreach (var component in binaryBlueprint.Components) {
                if (component == null) {
                    continue;
                }

                var type = component.FullTypeName.ToType();
                var index = Array.IndexOf(context.contextInfo.componentTypes, type);

                if (index != component.Index) {
                    Debug.Log(
                        string.Format(
                            "Blueprint '{0}' has invalid or outdated component index for '{1}'. Index was {2} but should be {3}. Updated index.",
                            binaryBlueprint.name,
                            component.FullTypeName,
                            component.Index,
                            index));

                    component.Index = index;
                    needsUpdate = true;
                }
            }

            return needsUpdate;
        }

        private static IContext[] FindAllContexts() {
            var contextsType = AppDomain.CurrentDomain
                .GetNonAbstractTypes<IContexts>()
                .SingleOrDefault();
            if (contextsType == null) {
                return null;
            }

            var contexts = (IContexts) Activator.CreateInstance(contextsType);
            return contexts.allContexts;
        }

        private EntityBlueprint blueprint;

        private IContext[] allContexts;
        private string[] allContextNames;
        private int contextIndex;

        private IContext context;
        private IEntity entity;

        private void Awake() {
            allContexts = FindAllContexts();
            if (allContexts == null) {
                return;
            }

            var binaryBlueprint = ((EntityBlueprint) target);

            allContextNames = allContexts.Select(context => context.contextInfo.name).ToArray();

            UpdateEntityBlueprint(binaryBlueprint, allContexts, allContextNames);

            blueprint = binaryBlueprint;

            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(target), blueprint.name);

            contextIndex = Array.IndexOf(allContextNames, blueprint.ContextIdentifier);
            SwitchToContext();

            entity.ApplyBlueprint(blueprint);

            // Serialize in case the structure of a component changed, e.g. field got removed
            binaryBlueprint.Serialize(entity);
        }

        private void OnDisable() {
            if (context != null) {
                context.Reset();
            }
        }

        public override void OnInspectorGUI() {
            var binaryBlueprint = ((EntityBlueprint) target);

            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.LabelField("Blueprint", EditorStyles.boldLabel);
                if (context != null) {
                    EditorGUILayout.BeginHorizontal();
                    {
                        contextIndex = EditorGUILayout.Popup(contextIndex, allContextNames);

                        if (EditorLayout.MiniButton("Switch Context")) {
                            SwitchToContext();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EntityDrawer.DrawComponents(context, entity);
                } else {
                    EditorGUILayout.LabelField("No contexts found!");
                }
            }
            var changed = EditorGUI.EndChangeCheck();
            if (changed) {
                binaryBlueprint.Serialize(entity);
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(target), binaryBlueprint.name);
                EditorUtility.SetDirty(target);
            }
        }

        private void SwitchToContext() {
            if (context != null) {
                context.Reset();
            }

            var targetContext = allContexts[contextIndex];
            context = (IContext) Activator.CreateInstance(targetContext.GetType());
            entity = (IEntity) context.GetType().GetMethod("CreateEntity").Invoke(context, null);
        }
    }
}