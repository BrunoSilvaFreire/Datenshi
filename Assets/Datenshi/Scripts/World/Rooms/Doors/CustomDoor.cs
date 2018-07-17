using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Doors {
    public class CustomDoor : AbstractDoor {
        public Animator Animator;
        public string OpenKey = "Open";
        public string CloseKey = "Close";

        public override void Open(bool silent = false) {
            Animator.SetTrigger(OpenKey);
        }

        public override void Close(bool silent = false) {
            Animator.SetTrigger(CloseKey);
        }
    }
}