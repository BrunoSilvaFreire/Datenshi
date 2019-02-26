using Datenshi.Scripts.Game.Restart;

namespace Datenshi.Scripts.World.Rooms.Game.Doors {
    public abstract class AbstractDoor : AbstractRoomMember, IRestartable {
        public bool BeginOpen;

        private void Start() {
            if (BeginOpen) {
                Open(true);
            } else {
                Close(true);
            }
        }

        public abstract void Open(bool silent = false);
        public abstract void Close(bool silent = false);
        public void Restart() { }
    }
}