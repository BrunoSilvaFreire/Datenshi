using System.Collections;
using Datenshi.Scripts.Cutscenes.Dialogue.UI;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;
using UnityEngine.Events;

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

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var stage = UIMainDialogueStage.Instance;
            if (OpenOnStart) {
                stage.Showing = true;
            }

            stage.SetShowContinueInstruction(true);
            yield return stage.PlayDialogue(Dialogue);
            if (CloseOnFinish) {
                stage.Showing = false;
            }

            stage.SetShowContinueInstruction(false);
        }

    }
}