using UnityEngine;

namespace Shiroi.Cutscenes.Triggers {
    public abstract class CutsceneTrigger : MonoBehaviour {
        public Cutscene Cutscene;
        public CutscenePlayer Player;
        public bool AllowReplay;
        protected void Trigger() {
            if (!Player) {
                Debug.LogWarning("[ShiroiCutscenes] Couldn't find an active instance of CutscenePlayer!");
                return;
            }

            
            Player.Play(Cutscene);
            if (!AllowReplay) {
             Destroy(gameObject);   
            }
        }
    }
}