using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public class EntityProxy : MonoBehaviour {
        public Entity Target;


        public void SetCharacter(Character.Character value) {
            Target.Character = value;
        }

        public void SetUseGUILayout(bool value) {
            Target.useGUILayout = value;
        }

#if UNITY_EDITOR
        public void SetRunInEditMode(bool value) {
            Target.runInEditMode = value;
        }
#endif

        public void SetEnabled(bool value) {
            Target.enabled = value;
        }

        public void SetTag(string value) {
            Target.tag = value;
        }

        public void SetName(string value) {
            Target.name = value;
        }

        public void SetHideFlags(HideFlags value) {
            Target.hideFlags = value;
        }
    }
}