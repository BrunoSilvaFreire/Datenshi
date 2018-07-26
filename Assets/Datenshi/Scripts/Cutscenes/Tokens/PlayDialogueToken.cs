using System;
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
            UIDialogueStage stage;
            switch (Target) {
                case DialogueTarget.Main:
                    stage = UIMainDialogueStage.Instance;
                    break;
                case DialogueTarget.World:
                    stage = UIWorldDialogueStage.Instance;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (OpenOnStart) {
                stage.Showing = true;
            }

            yield return stage.PlayDialogue(Dialogue);
            if (CloseOnFinish) {
                stage.Showing = false;
            }
        }
    }
}