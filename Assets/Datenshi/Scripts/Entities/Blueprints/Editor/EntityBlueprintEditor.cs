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
            return AssetDatabase.FindAssets("l:" + EntityBlueprintPostprocessor.ASSET_LABEL)
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<EntityBlueprint>(path))
                .ToArray();
        }

        [DidReloadScripts, MenuItem("Tools/Entitas/Blueprints/Update all Blueprints", false, 300)]
        public static void UpdateAllEntityBlueprints() {
            if (!EditorApplication.isPlayingOrWillChangePlaymode) {
                var allContexts = findAllContexts();
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
                    Debug.Log("Validated " + binaryBlueprints.Length + " Blueprints, " + updated + " have been updated.");
                }
            }
        }

        public static bool UpdateEntityBlueprint(EntityBlueprint binaryBlueprint, IContext[] allContexts, string[] allContextNames) {
            var needsUpdate = false;

            var contextIndex = Array.IndexOf(allContextNames, binaryBlueprint.ContextIdentifier);
            if (contextIndex < 0) {
                contextIndex = 0;
                needsUpdate = true;
            }

            var context = allContexts[contextIndex];
            binaryBlueprint.ContextIdentifier = context.contextInfo.name;

            foreach (var component in binaryBlueprint.Components) {
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

            /*if (needsUpdate) {
                Debug.Log("Updating Blueprint '" + binaryBlueprint.name + "'");
                binaryBlueprint.Serialize(binaryBlueprint);
            }*/

            return needsUpdate;
        }

        static IContext[] findAllContexts() {
            var contextsType = AppDomain.CurrentDomain
                .GetNonAbstractTypes<IContexts>()
                .SingleOrDefault();
            if (contextsType != null) {
                var contexts = (IContexts) Activator.CreateInstance(contextsType);
                return contexts.allContexts;
            }

            return null;
        }

        EntityBlueprint _blueprint;

        IContext[] _allContexts;
        string[] _allContextNames;
        int _contextIndex;

        IContext _context;
        IEntity _entity;

        void Awake() {
            _allContexts = findAllContexts();
            if (_allContexts == null) {
                return;
            }

            var binaryBlueprint = ((EntityBlueprint) target);

            _allContextNames = _allContexts.Select(context => context.contextInfo.name).ToArray();

            UpdateEntityBlueprint(binaryBlueprint, _allContexts, _allContextNames);

            _blueprint = binaryBlueprint;

            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(target), _blueprint.name);

            _contextIndex = Array.IndexOf(_allContextNames, _blueprint.ContextIdentifier);
            switchToContext();

            _entity.ApplyBlueprint(_blueprint);

            // Serialize in case the structure of a component changed, e.g. field got removed
            binaryBlueprint.Serialize(_entity);
        }

        void OnDisable() {
            if (_context != null) {
                _context.Reset();
            }
        }

        public override void OnInspectorGUI() {
            var binaryBlueprint = ((EntityBlueprint) target);

            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.LabelField("Blueprint", EditorStyles.boldLabel);
                if (_context != null) {
                    EditorGUILayout.BeginHorizontal();
                    {
                        _contextIndex = EditorGUILayout.Popup(_contextIndex, _allContextNames);

                        if (EditorLayout.MiniButton("Switch Context")) {
                            switchToContext();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EntityDrawer.DrawComponents(_context, _entity);
                } else {
                    EditorGUILayout.LabelField("No contexts found!");
                }
            }
            var changed = EditorGUI.EndChangeCheck();
            if (changed) {
                binaryBlueprint.Serialize(_entity);
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(target), binaryBlueprint.name);
                EditorUtility.SetDirty(target);
            }
        }

        void switchToContext() {
            if (_context != null) {
                _context.Reset();
            }
            var targetContext = _allContexts[_contextIndex];
            _context = (IContext) Activator.CreateInstance(targetContext.GetType());
            _entity = (IEntity) _context.GetType().GetMethod("CreateEntity").Invoke(_context, null);
        }
    }
}