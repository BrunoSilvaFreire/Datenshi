namespace Datenshi.Scripts.World.Rooms.Doors {
    public abstract class AbstractDoor : AbstractRoomMember {
        public abstract void Open(bool silent = false);
        public abstract void Close(bool silent = false);
    }
}