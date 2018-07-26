using System.Collections;
using Datenshi.Scripts.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Datenshi.Scripts.Cutscenes.Dialogue.UI {
    public abstract class UIDialogueStage : UIMenu {
        public Dialogue CurrentDialogue {
            get;
            private set;
        }

        public IEnumerator PlayDialogue(Dialogue dialogue) {
            if (CurrentDialogue != null) {
                yield break;
            }

            CurrentDialogue = dialogue;
            yield return DoPlayDialogue(dialogue);
            CurrentDialogue = null;
        }

        protected abstract IEnumerator DoPlayDialogue(Dialogue dialogue);
    }

    public abstract class UIDialogueStage<T> : UIDialogueStage where T : Object {
        private static T instance;

        public static T Instance => instance ? instance : (instance = FindObjectOfType<T>());
    }
}