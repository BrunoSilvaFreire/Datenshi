using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public class EntityProxy : MonoBehaviour {
        public Entity Target;

        public void SnapToFloor() {
            Target.SnapToFloor();
        }

        public void SetInputProvider(InputProvider value) {
            Target.InputProvider = value;
        }

        public void SetHitbox(Collider2D value) {
            Target.Hitbox = value;
        }

        public void SetCurrentDirection(Direction value) {
            Target.CurrentDirection = value;
        }

        public void SetUseGUILayout(bool value) {
            Target.useGUILayout = value;
        }

        public void SetRunInEditMode(bool value) {
            Target.runInEditMode = value;
        }

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