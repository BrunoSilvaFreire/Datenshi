using System.Collections;
using System.Collections.Generic;

namespace Datenshi.Scripts.Cutscenes.Dialogue.UI {
    public class UIWorldDialogueStage : UIDialogueStage<UIWorldDialogueStage> {
        protected override void SnapShow() {
            foreach (var portrait in Elements) {
                portrait.SnapShowing(true);
            }
        }

        protected override void SnapHide() {
            foreach (var portrait in Elements) {
                portrait.SnapShowing(false);
            }
        }

        protected override void OnShow() {
            foreach (var portrait in Elements) {
                portrait.Showing = true;
            }
        }

        protected override void OnHide() {
            foreach (var portrait in Elements) {
                portrait.Showing = false;
            }
        }

        private Dialogue currentDialogue;
        private List<DialogueHistory> lastLines = new List<DialogueHistory>();

        private sealed class DialogueHistory {
            public DialogueHistory(DialogueSpeech speech, DialogueLine line) {
                Speech = speech;
                Line = line;
            }

            public DialogueSpeech Speech {
                get;
            }

            public DialogueLine Line {
                get;
            }
        }

        protected override IEnumerator DoPlayDialogue(Dialogue dialogue) {
            var speeches = dialogue.Speeches;
            foreach (var speech in speeches) {
                var character = speech.Character;
                foreach (var line in speech.Lines) {
                    var history = new DialogueHistory(speech, line);
                    lastLines.Add(history);
                    yield return AddLine(history);
                }
            }
        }

        private IEnumerator AddLine(DialogueHistory history) {
            throw new System.NotImplementedException();
        }
    }
}