using Datenshi.Scripts.Math;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Editor {
    /*[CustomPropertyDrawer(typeof(BezierCurve))]
    public class BezierCurveDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUILayout.PropertyField(
                property.FindPropertyRelative(nameof(BezierCurve.Points)),
                label,
                true
            );

            DrawBezier(property);
            EditorGUI.EndProperty();
        }

        private void DrawBezier(SerializedProperty property) {
            var points = property.FindPropertyRelative(nameof(BezierCurve.Points));
            for (var i = 0; i < points.arraySize - 1; i++) {
                var a = points.GetArrayElementAtIndex(i);
                var b = points.GetArrayElementAtIndex(i + 1);
                var startPos = a.FindPropertyRelative(nameof(BezierPoint.Position)).vector3Value;
                var endPos = b.FindPropertyRelative(nameof(BezierPoint.Position)).vector3Value;
                var startTangent = a.FindPropertyRelative(nameof(BezierPoint.StartTangent)).vector3Value;
                var endTangent = b.FindPropertyRelative(nameof(BezierPoint.EndTangent)).vector3Value;
                Debug.Log($"Bezier {startPos}, {endPos}, {startTangent}, {endTangent}");
                Handles.DrawBezier(startPos, endPos, startTangent, endTangent, Color.green, Texture2D.whiteTexture, 1);
            }
        }
    }*/
}