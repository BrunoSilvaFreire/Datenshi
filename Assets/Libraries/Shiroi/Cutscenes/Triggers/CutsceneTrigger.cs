using UnityEngine;

namespace Shiroi.Cutscenes.Triggers {
    public abstract class CutsceneTrigger : MonoBehaviour {
        public Cutscene Cutscene;
        public CutscenePlayer Player;
        public bool AllowReplay;
        private bool played;
        protected void Trigger() {
            if (!Player) {
                Debug.LogWarning("[ShiroiCutscenes] Couldn't find an active instance of CutscenePlayer!");
                return;
            }

            if (!AllowReplay && played) {
                return;
            }
            Debug.Log("Log cutscenes @ " + Cutscene + " @ " + Player);
            Player.Play(Cutscene);
            played = true;
            if (!AllowReplay) {
                Destroy(this);
            }
        }
    }
}