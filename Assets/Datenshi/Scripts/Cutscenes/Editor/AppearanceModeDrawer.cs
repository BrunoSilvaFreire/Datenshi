using System;
using System.Reflection;
using Datenshi.Scripts.Character;
using Datenshi.Scripts.UI.Dialogue;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Editor;
using Shiroi.Cutscenes.Editor.Util;
using Shiroi.Cutscenes.Editor.Drawers;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Editor {
    public class AppearanceModeDrawer : TypeDrawer<AppearanceMode> {
        public override uint GetTotalLines() {
            return 4;
        }

        public override void Draw(
            CutsceneEditor editor,
            CutscenePlayer player,
            Cutscene cutscene,
            Rect rect,
            int tokenIndex,
            GUIContent name,
            AppearanceMode value,
            Type valueType,
            FieldInfo fieldInfo,
            Setter setter
        ) {
            var a = rect.GetLine(0);
            var b = rect.GetLine(1);
            var c = rect.GetLine(2);
            var d = rect.GetLine(3);
            EditorGUI.LabelField(a, name);
            value.Duration = EditorGUI.FloatField(b, "Duration", value.Duration);
            value.Offset = EditorGUI.FloatField(c, "Offset", value.Offset);
            value.Left = EditorGUI.Toggle(d, "Left", value.Left);
            setter(value);
        }
    }
}