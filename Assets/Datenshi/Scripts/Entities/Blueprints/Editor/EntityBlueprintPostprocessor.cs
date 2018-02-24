using System.Collections.Generic;
using UnityEditor;

namespace Datenshi.Scripts.Entities.Blueprints.Editor {
    public class EntityBlueprintPostprocessor : AssetPostprocessor {
        public const string AssetLabel = "EntityBlueprint";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromPath) {
            foreach (var assetPath in importedAssets) {
                var asset = AssetDatabase.LoadAssetAtPath<EntityBlueprint>(assetPath);
                if (asset == null) {
                    continue;
                }

                var labels = new List<string>(AssetDatabase.GetLabels(asset));
                if (labels.Contains(AssetLabel)) {
                    continue;
                }

                labels.Add(AssetLabel);
                AssetDatabase.SetLabels(asset, labels.ToArray());
            }
        }
    }
}