using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Game.Doors {
    public class AnimatedDoor : AbstractDoor {
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