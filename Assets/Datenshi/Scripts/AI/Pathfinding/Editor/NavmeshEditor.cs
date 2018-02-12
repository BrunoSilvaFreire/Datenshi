using System;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Editor;
using DesperateDevs.Unity.Editor;
using Shiroi.Cutscenes.Util;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Editor {
    [CustomEditor(typeof(Navmesh))]
    public class NavmeshEditor : UnityEditor.Editor {
        public const float ColorAlpha = 0.2F;
        private static readonly Color BlockedColor = new Color(0.96f, 0.26f, 0.21f, ColorAlpha);
        private static readonly Color PlatformColor = new Color(0.25f, 0.32f, 0.71f, ColorAlpha);
        private static readonly Color SoloColor = new Color(0.91f, 0.12f, 0.39f, ColorAlpha);
        private static readonly Color EdgeColor = new Color(1f, 0.76f, 0.03f, ColorAlpha);
        private static readonly Color EmptyColor = new Color(1, 1, 1, ColorAlpha);
        private static readonly Color LinearLinksColor = new Color(0f, 0.9f, 1f, ColorAlpha);
        private static readonly Color GravityLinksColor = new Color(0.46f, 1f, 0.01f, ColorAlpha);

        public Color GetColor(NodeType NodeType) {
            switch (NodeType) {
                case NodeType.Blocked:
                    return BlockedColor;
                case NodeType.Platform:
                    return PlatformColor;
                case NodeType.Solo:
                    return SoloColor;
                case NodeType.RightEdge:
                case NodeType.LeftEdge:
                    return EdgeColor;
                case NodeType.Empty:
                    return EmptyColor;
            }
            throw new ArgumentOutOfRangeException("NodeType", NodeType, null);
        }

        private Navmesh navmesh;

        private void OnEnable() {
            navmesh = (Navmesh) target;
        }

        public override void OnInspectorGUI() {
            EditorGUI.BeginChangeCheck();
            //EditorGUILayout.BeginHorizontal();
            navmesh.Min = EditorGUILayout.Vector2IntField("Min", navmesh.Min);
            navmesh.Max = EditorGUILayout.Vector2IntField("Max", navmesh.Max);
            //EditorGUILayout.EndHorizontal();
            LayerMask tempMask = EditorGUILayout.MaskField("LayerMask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(navmesh.LayerMask), InternalEditorUtility.layers);
            navmesh.LayerMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);

            navmesh.Grid = (Grid) EditorGUILayout.ObjectField("Grid", navmesh.Grid, typeof(Grid), true);
            if (EditorLayout.MiniButton("Regenerate")) {
                navmesh.Regenerate();
            }
            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(navmesh);
            }
        }

        private void OnSceneGUI() {
            DrawSceneNodes();
            DrawSceneLinks();
            DrawSceneBounds();
        }

        private void DrawSceneNodes() {
            var grid = navmesh.Grid;
            var cellSize = grid.cellSize;
            foreach (var node in navmesh.Nodes) {
                if (node.IsInvalid) {
                    continue;
                }
                var color = GetColor(node.Type);
                var center = grid.CellToWorld(node.Position.ToVector3()) + cellSize / 2;
                HandlesUtil.DrawBox2D(center, cellSize, color);
            }
        }


        private void DrawSceneBounds() {
            var grid = navmesh.Grid;
            if (grid == null) {
                return;
            }
            var size = grid.cellSize;
            var min = navmesh.Min;
            var max = navmesh.Max;
            var minCenter = grid.GetCellCenterWorld(min.ToVector3());
            var maxCenter = grid.GetCellCenterWorld(max.ToVector3());
            navmesh.Min = grid.WorldToCell(Handles.PositionHandle(minCenter, Quaternion.identity)).ToVector2();
            navmesh.Max = grid.WorldToCell(Handles.PositionHandle(maxCenter, Quaternion.identity)).ToVector2();
            HandlesUtil.DrawBox2DWire(navmesh.Center, navmesh.Size, Color.green);
            HandlesUtil.DrawBox2DWire(minCenter, grid.cellSize, Color.cyan);
            HandlesUtil.DrawBox2DWire(maxCenter, grid.cellSize, Color.magenta);
        }

        private void DrawSceneLinks() {
            var links = navmesh.Links;
            if (links == null) {
                return;
            }
            foreach (var link in links) {
                link.DrawGizmos(navmesh);
            }
        }
    }
}