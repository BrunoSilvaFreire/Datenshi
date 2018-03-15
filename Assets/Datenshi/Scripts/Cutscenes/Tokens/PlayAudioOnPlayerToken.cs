using System.Collections;
using Datenshi.Scripts.Misc;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
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
}