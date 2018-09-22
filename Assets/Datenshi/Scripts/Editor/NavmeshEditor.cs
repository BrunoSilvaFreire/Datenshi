using System;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.AI.Pathfinding.Links.Generators;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;

namespace Datenshi.Scripts.Editor {
    [CustomEditor(typeof(Navmesh))]
    public class NavmeshEditor : UnityEditor.Editor {
        private const float ColorAlpha = 0.2F;
        private static readonly Color BlockedColor = new Color(0.96f, 0.26f, 0.21f, ColorAlpha);
        private static readonly Color PlatformColor = new Color(0.25f, 0.32f, 0.71f, ColorAlpha);
        private static readonly Color SoloColor = new Color(0.91f, 0.12f, 0.39f, ColorAlpha);
        private static readonly Color EdgeColor = new Color(1f, 0.76f, 0.03f, ColorAlpha);
        private static readonly Color EmptyColor = new Color(0.48f, 0.48f, 0.48f, 0.2f);
        private static readonly Color LinearLinksColor = new Color(0f, 0.9f, 1f, ColorAlpha);
        private static readonly Color GravityLinksColor = new Color(0.46f, 1f, 0.01f, ColorAlpha);
        private static GUIStyle style;
        private float visualizerPrecision;

        private static GUIStyle Style => style ?? (style = new GUIStyle {
            normal = {
                textColor = Color.white,
                background = Texture2D.blackTexture,
            },
        });

        private static Color GetColor(NodeType NodeType) {
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
        private bool showGenerators = true;
        private bool showOptions = true;
        private bool showEmptyNodes;
        private bool showLinks = true;
        private bool showMouseInfo = true;
        private bool showBlockedNodes = true;
        private bool precisionChanged;

        private void OnEnable() {
            navmesh = (Navmesh) target;
            visualizerPrecision = EditorPrefs.GetFloat(Constants.PrecisionConfigKey);
        }

        public override void OnInspectorGUI() {
            var changed = false;
            showGenerators = EditorGUILayout.Foldout(showGenerators, "Generators");
            if (showGenerators) {
                foreach (var linkGenerator in LinkGenerators.Generators) {
                    EditorGUILayout.LabelField(linkGenerator.GetType().Name);
                    linkGenerator.DrawEditor();
                }
            }

            showOptions = EditorGUILayout.Foldout(showOptions, "Options");
            precisionChanged = false;
            if (showOptions) {
                //Precision config
                EditorGUI.BeginChangeCheck();
                visualizerPrecision = EditorGUILayout.Slider("Precision", visualizerPrecision,
                    GameResources.Instance.MinPrecision, GameResources.Instance.MaxPrecision);
                precisionChanged = EditorGUI.EndChangeCheck();
                EditorPrefs.SetFloat(Constants.PrecisionConfigKey, visualizerPrecision);


                showEmptyNodes = EditorGUILayout.Toggle("Show Empty Nodes", showEmptyNodes);
                showBlockedNodes = EditorGUILayout.Toggle("Show Blocked Nodes", showBlockedNodes);
                showLinks = EditorGUILayout.Toggle("Show Links", showLinks);
                showMouseInfo = EditorGUILayout.Toggle("Show Mouse Info", showMouseInfo);
            }

            EditorGUI.BeginChangeCheck();
            //EditorGUILayout.BeginHorizontal();
            navmesh.Min = EditorGUILayout.Vector2IntField("Min", navmesh.Min);
            navmesh.Max = EditorGUILayout.Vector2IntField("Max", navmesh.Max);
            //EditorGUILayout.EndHorizontal();
            LayerMask tempMask = EditorGUILayout.MaskField("LayerMask",
                InternalEditorUtility.LayerMaskToConcatenatedLayersMask(navmesh.LayerMask),
                InternalEditorUtility.layers);
            navmesh.LayerMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
            navmesh.Grid = (Grid) EditorGUILayout.ObjectField("Grid", navmesh.Grid, typeof(Grid), true);
            GUI.enabled = navmesh.Grid;
            if (GUILayout.Button("Regenerate")) {
                navmesh.Regenerate();
                GenerateLinks();
                changed = true;
            }

            if (EditorGUI.EndChangeCheck()) {
                changed = true;
            }

            if (changed) {
                EditorSceneManager.MarkSceneDirty(navmesh.gameObject.scene);
            }
        }

        private void GenerateLinks() {
            var grid = navmesh.Grid;
            if (grid == null) {
                return;
            }

            foreach (var node in navmesh.Nodes) {
                foreach (var generator in LinkGenerators.Generators) {
                    var center = grid.GetCellCenterWorld(node.Position.ToVector3());
                    foreach (var link in generator.Generate(node, navmesh, center, visualizerPrecision)) {
                        node.AddLink(link);
                    }
                }
            }
        }

        private void OnSceneGUI() {
            if (navmesh.Grid == null) {
                return;
            }

            DrawSceneNodes();
            if (showLinks) {
                DrawSceneLinks();
            }

            DrawSceneBounds();
            if (showMouseInfo) {
                DrawMouseInfo();
            }
        }

        public Vector2 ConvertToWorldPos(Vector2 pos, Camera camera) {
            var position = pos;
            position.y = camera.pixelHeight - position.y;
            position = camera.ScreenToWorldPoint(position);
            return position;
        }

        private void DrawMouseInfo() {
            //Do mouse over
            var selectedGameView = EditorWindow.mouseOverWindow as SceneView;
            if (!selectedGameView) {
                return;
            }

            Vector3 mousePosition = Event.current.mousePosition;
            var camera = selectedGameView.camera;
            var worldPosition = ConvertToWorldPos(mousePosition, camera);
            //Draw mouse Positions
            Node node = GetNode(worldPosition);
            var size = navmesh.Grid.cellSize;
            DrawMousePositions(mousePosition, worldPosition, camera, node);
            //Draw world
            if (node.IsInvalid) {
                return;
            }

            var center = navmesh.WorldPosCenter(node);
            //HandlesUtil.DrawBox2D(center, Navmesh.BoxCastSize, Color.cyan);
            if (node.IsEmpty) {
                //HandlesUtil.DrawBox2D(center, size, Color.white);
                return;
            }

            var nodeColor = node.IsWalkable ? Color.green : Color.red;
            //HandlesUtil.DrawBox2DWire(center, size, nodeColor);
            var e = Event.current;
            /*bool showGLinkInfos;
            if (e.type == EventType.KeyDown) {
                showGLinkInfos = e.keyCode == KeyCode.LeftShift;
            } else {
                showGLinkInfos = false;
            }*/
        }

        private void TextRaw(Vector2 center, string getDebugString) {
            Handles.Label(center, getDebugString, Style);
        }

        private void Text(string text, Vector3 pos, Camera camera) {
            TextRaw(ConvertToWorldPos(pos, camera), text);
        }

        private void DrawMousePositions(
            Vector3 mousePosition,
            Vector2 worldPosition,
            Camera camera,
            Node node) {
            var mousePosText = "Mouse position = " + mousePosition;
            var mouseDisplayPostition = mousePosition;
            mouseDisplayPostition.x += 20;
            Text(mousePosText, mouseDisplayPostition, camera);
            var height = EditorGUIUtility.singleLineHeight;
            mouseDisplayPostition.y += height;
            Text("World position = " + worldPosition, mouseDisplayPostition, camera);
            mouseDisplayPostition.y += height;
            if (node == null) {
                Text("Out of bounds!", mouseDisplayPostition, camera);
            } else {
                var runLinks = node.TotalLinearLinks;
                var msg = string.Format("{0} = {1}", navmesh.GetNodeIndex(node), node);
                Text(msg, mouseDisplayPostition, camera);
                mouseDisplayPostition.y += height;
                Text("Linear links: " + runLinks, mouseDisplayPostition, camera);
                var gravityLinks = node.TotalGravityLinks;
                mouseDisplayPostition.y += height;
                Text("Gravity links: " + gravityLinks, mouseDisplayPostition, camera);
                if (!showLinks) {
                    return;
                }

                if (node.IsInvalid) {
                    return;
                }

                var nodeIndex = navmesh.GetNodeIndex(node);
                foreach (var link in node.Links) {
                    if (link.DrawOnlyOnMouseOver()) {
                        link.DrawGizmos(navmesh, (uint) nodeIndex, visualizerPrecision, precisionChanged);
                    }
                }
            }
        }

        private Node GetNode(Vector2 worldPosition) {
            if (navmesh.IsOutOfBounds(worldPosition)) {
                return Node.Invalid;
            }

            return navmesh.GetNodeAtWorld(worldPosition);
        }

        private void DrawSceneNodes() {
            var grid = navmesh.Grid;
            if (grid == null) {
                return;
            }

            var cellSize = grid.cellSize;
            foreach (var node in navmesh.Nodes) {
                if (node.IsInvalid) {
                    continue;
                }

                if (!showEmptyNodes && node.IsEmpty) {
                    continue;
                }

                if (!showBlockedNodes && node.IsBlocked) {
                    continue;
                }

                var color = GetColor(node.Type);
                var center = grid.CellToWorld(node.Position.ToVector3()) + cellSize / 2;
                //HandlesUtil.DrawBox2D(center, cellSize, color);
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
            /*HandlesUtil.DrawBox2DWire(navmesh.Center, navmesh.Size, Color.green);
            HandlesUtil.DrawBox2DWire(minCenter, grid.cellSize, Color.cyan);
            HandlesUtil.DrawBox2DWire(maxCenter, grid.cellSize, Color.magenta);*/
        }

        private void DrawSceneLinks() {
            for (uint i = 0; i < navmesh.TotalSize; i++) {
                var node = navmesh[i];
                foreach (var link in node.Links) {
                    if (link.DrawOnlyOnMouseOver()) {
                        continue;
                    }

                    link.DrawGizmos(navmesh, i, visualizerPrecision, precisionChanged);
                }
            }
        }
    }
}