﻿using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Tile.Editor {
    //[CreateAssetMenu]
    [CustomGridBrush(false, true, false, "Prefab Brush")]
    public class PrefabBrush : GridBrushBase {
        private const float k_PerlinOffset = 100000f;
        public GameObject[] m_Prefabs;
        public float m_PerlinScale = 0.5f;
        public int m_Z;

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            var index = Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, m_PerlinScale, k_PerlinOffset) * m_Prefabs.Length), 0, m_Prefabs.Length - 1);
            var prefab = m_Prefabs[index];
            var instance = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
            Undo.RegisterCreatedObjectUndo((Object) instance, "Paint Prefabs");
            if (instance != null) {
                instance.transform.SetParent(brushTarget.transform);
                instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, m_Z) + new Vector3(.5f, .5f, .5f)));
            }
        }

        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            var erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, m_Z));
            if (erased != null)
                Undo.DestroyObjectImmediate(erased.gameObject);
        }

        private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position) {
            var childCount = parent.childCount;
            var min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
            var max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
            var bounds = new Bounds((max + min) * .5f, max - min);

            for (var i = 0; i < childCount; i++) {
                var child = parent.GetChild(i);
                if (bounds.Contains(child.position))
                    return child;
            }
            return null;
        }

        private static float GetPerlinValue(Vector3Int position, float scale, float offset) {
            return Mathf.PerlinNoise((position.x + offset) * scale, (position.y + offset) * scale);
        }
    }

    [CustomEditor(typeof(PrefabBrush))]
    public class PrefabBrushEditor : GridBrushEditorBase {
        private PrefabBrush prefabBrush => target as PrefabBrush;

        private SerializedProperty m_Prefabs;
        private SerializedObject m_SerializedObject;

        protected void OnEnable() {
            m_SerializedObject = new SerializedObject(target);
            m_Prefabs = m_SerializedObject.FindProperty("m_Prefabs");
        }

        public override void OnPaintInspectorGUI() {
            m_SerializedObject.UpdateIfRequiredOrScript();
            prefabBrush.m_PerlinScale = EditorGUILayout.Slider("Perlin Scale", prefabBrush.m_PerlinScale, 0.001f, 0.999f);
            prefabBrush.m_Z = EditorGUILayout.IntField("Position Z", prefabBrush.m_Z);

            EditorGUILayout.PropertyField(m_Prefabs, true);
            m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}