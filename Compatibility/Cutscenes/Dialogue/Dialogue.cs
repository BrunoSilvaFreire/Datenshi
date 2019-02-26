using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Dialogue {
    [CreateAssetMenu(menuName = "Datenshi/Cutscenes/Dialogue")]
    public class Dialogue : ScriptableObject {
        public DialogueSpeech[] Speeches;
    }

    [Serializable]
    public class DialogueSpeech {
        public Character.Character Character;
        public DialogueLine[] Lines;
    }

    [Serializable]
    public class DialogueLine {
        [TextArea]
        public string Text;

        public bool Move;

        [ShowIf("Move")]
        public AppearanceMode AppearanceMode;

        public override string ToString() {
            return $"Dialogue(Text: {Text}, Move: {Move}, AppearanceMode: {AppearanceMode})";
        }
    }
}