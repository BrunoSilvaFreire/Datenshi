using System;
using Datenshi.Scripts.UI.Dialogue;
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
        public string Text;
        public bool Move;

        [ShowIf("Move")]
        public AppearanceMode AppearanceMode;
    }
}