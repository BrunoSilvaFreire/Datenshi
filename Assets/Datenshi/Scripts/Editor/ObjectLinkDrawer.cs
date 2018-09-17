using Datenshi.Scripts.Util;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Editor {
    [CustomPropertyDrawer(typeof(ObjectLink))]
    public class ObjectLinkDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var idProp = property.FindPropertyRelative("id");
            position.height = EditorGUIUtility.singleLineHeight;
            var objectPos = position;
            objectPos.y += EditorGUIUtility.singleLineHeight;
            var oldValue = idProp.stringValue;
            var originalString = oldValue.Split(':')[0];
            idProp.stringValue = new PropertyName(
                EditorGUI.TextField(position, "Key", originalString)
            ).ToString();
            EditorGUI.PropertyField(objectPos, property.FindPropertyRelative("obj"), new GUIContent(idProp.stringValue));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight * 2;
        }
    }
}