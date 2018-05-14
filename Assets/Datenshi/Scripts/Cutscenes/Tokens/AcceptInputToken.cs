using System.Collections;
using Datenshi.Scripts.Cutscenes.Dialogue.UI;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class AcceptInputToken : IToken {
        public enum AcceptInputTarget {
            PlayerOnly,
            AIOnly,
            All
        }

        public bool AcceptInput;
        public AcceptInputTarget Target = AcceptInputTarget.All;

        public IEnumerator Execute(CutscenePlayer player) {
            if (Target == AcceptInputTarget.AIOnly || Target == AcceptInputTarget.All) {
                RuntimeResources.Instance.AllowAIInput = AcceptInput;
            }

            if (Target == AcceptInputTarget.PlayerOnly || Target == AcceptInputTarget.All) {
                RuntimeResources.Instance.AllowPlayerInput = AcceptInput;
            }

            yield break;
        }
    }

    public class PlayAudioOnPlayerToken : IToken {
        public AudioClip AudioClip;
        public bool DeleteOnFinish;
        public bool Wait;

        public IEnumerator Execute(CutscenePlayer player) {
            var go = Camera.main.gameObject;
            var source = go.AddComponent<AudioSource>();
            source.clip = AudioClip;
            source.Play();
            if (DeleteOnFinish) {
                var b = go.AddComponent<Dispatcher>();
                b.StartCoroutine(DeleteDelayed(AudioClip.length, source, b));
            }

            if (Wait) {
                yield return new WaitForSeconds(AudioClip.length);
            }

            yield break;
        }

        private IEnumerator DeleteDelayed(float audioClipLength, AudioSource source, MonoBehaviour monoBehaviour) {
            yield return new WaitForSeconds(audioClipLength);
            Object.Destroy(source);
            Object.Destroy(monoBehaviour);
        }
    }

    public class PlayDialogueToken : IToken {
        public Dialogue.Dialogue Dialogue;

        public enum DialogueTarget {
            Main,
            World
        }

        public DialogueTarget Target;
        public bool CloseOnFinish;

        public IEnumerator Execute(CutscenePlayer player) {
            var stage = UIMainDialogueStage.Instance;
            //TODO fix
            yield return stage.PlayDialogue(Dialogue);
            if (CloseOnFinish) {
                stage.Showing = false;
            }

            yield break;
        }
    }
}