using Datenshi.Scripts.Math;
using Datenshi.Scripts.World.Rooms.Game;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Editor {
    [CustomEditor(typeof(MovableBody))]
    public class MovableBodyEditor : UnityEditor.Editor {
        private MovableBody body;
        private float curveWidth;
        private float previewPosition;

        private void OnEnable() {
            body = target as MovableBody;
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            curveWidth = EditorGUILayout.Slider("Curve Width", curveWidth, 1, 10);
            previewPosition = EditorGUILayout.Slider("Preview", previewPosition, 0, 1);
        }

        private void OnSceneGUI() {
            var points = serializedObject.FindProperty(nameof(MovableBody.Curve))
                .FindPropertyRelative(nameof(BezierCurve.Points));
            var basePos = body.transform.position;
            var end = points.arraySize;
            for (var i = 0; i < end; i++) {
                var a = points.GetArrayElementAtIndex(i);
                var startPos = basePos + a.FindPropertyRelative(nameof(BezierPoint.Position)).vector3Value;
                Handles.color = Color.white;
                Handles.SphereHandleCap(0, startPos, Quaternion.identity, 1, EventType.Repaint);
                var s = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
                Handles.Label(startPos + Vector3.up * 2, $"Point #{i}",
                    s.FindStyle(GUISkinProperties.INGameObjectHeader));
                DrawHandles(a);
                if (i >= end - 1) {
                    continue;
                }

                var b = points.GetArrayElementAtIndex(i + 1);
                DrawBezier(a, b);
            }

            if (body.Loop && points.arraySize >= 2) {
                DrawBezier(points.GetArrayElementAtIndex(end - 1), points.GetArrayElementAtIndex(0));
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawBezier(SerializedProperty a, SerializedProperty b) {
            var basePos = body.transform.position;
            var startPos = basePos + a.FindPropertyRelative(nameof(BezierPoint.Position)).vector3Value;
            var endPos = basePos + b.FindPropertyRelative(nameof(BezierPoint.Position)).vector3Value;
            var startTangent = a.FindPropertyRelative(nameof(BezierPoint.StartTangent)).vector3Value;
            var endTangent = b.FindPropertyRelative(nameof(BezierPoint.EndTangent)).vector3Value;
            Handles.DrawBezier(startPos, endPos, startPos + startTangent, endPos + endTangent, Color.green,
                Texture2D.whiteTexture, curveWidth);
        }

        private void DrawHandles(SerializedProperty point) {
            var position = point.FindPropertyRelative(nameof(BezierPoint.Position));
            var basePos = body.transform.position;
            var pointPos = Handles.PositionHandle(basePos + position.vector3Value, Quaternion.identity);
            position.vector3Value = pointPos - basePos;
            var inTangent = point.FindPropertyRelative(nameof(BezierPoint.StartTangent));
            inTangent.vector3Value = Handles.PositionHandle(pointPos + inTangent.vector3Value, Quaternion.identity) -
                                     pointPos;
            var outTangent = point.FindPropertyRelative(nameof(BezierPoint.EndTangent));
            outTangent.vector3Value = Handles.PositionHandle(pointPos + outTangent.vector3Value, Quaternion.identity) -
                                      pointPos;
            Handles.color = Color.blue;
            Handles.DrawLine(pointPos, pointPos + inTangent.vector3Value);
            Handles.color = Color.red;
            Handles.DrawLine(pointPos, pointPos + outTangent.vector3Value);
        }
    }
}