using System.Collections;
using Datenshi.Scripts.Cutscenes.Dialogue.UI;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class PlayDialogueToken : Token {
        public Dialogue.Dialogue Dialogue;

        public enum DialogueTarget {
            Main,
            World
        }

        public DialogueTarget Target;
        public bool OpenOnStart = true;
        public bool CloseOnFinish = true;

        public override IEnumerator Execute(CutscenePlayer player) {
            var stage = UIMainDialogueStage.Instance;
            if (OpenOnStart) {
                stage.Showing = true;
            }

            Debug.Log("Start Dialogue");
            yield return stage.PlayDialogue(Dialogue);
            Debug.Log("Stopped Dialogue");
            if (CloseOnFinish) {
                stage.Showing = false;
            }
        }
    }
}