using Datenshi.Scripts.Game.Restart;

namespace Datenshi.Scripts.World.Rooms.Game.Doors {
    public abstract class AbstractDoor : AbstractRoomMember, IRestartable {
        public abstract void Open(bool silent = false);
        public abstract void Close(bool silent = false);
        public void Restart() {
        }
    }
}