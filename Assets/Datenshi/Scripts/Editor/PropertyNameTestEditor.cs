using Datenshi.Scripts.Misc;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Editor {
    [CustomEditor(typeof(PropertyNameTest))]
    public class PropertyNameTestEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var idProp = serializedObject.FindProperty(nameof(PropertyNameTest.PropertyName));
            var oldValue = idProp.stringValue;
            var originalString = oldValue.Split(':')[0];
            var text = EditorGUILayout.TextField("Key", originalString);
            var prop = new PropertyName(originalString);
            Debug.Log($"Prop = '{prop}'/'{oldValue}'@ '{text}'");
            idProp.stringValue = prop.ToString();
            EditorGUILayout.PropertyField(idProp);
            serializedObject.ApplyModifiedProperties();
            var t = (PropertyNameTest) target;
            Debug.Log($"Target property became: '{t.PropertyName}'");
        }
    }
}