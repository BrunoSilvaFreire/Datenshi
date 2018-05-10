using Datenshi.Scripts.World.Rooms;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.World {
    public class AbstractRoomMember : MonoBehaviour, IRoomMember {
        [SerializeField]
        private UnityEvent onDestroyed;

        public UnityEvent OnDestroyed => onDestroyed;

        public Room Room {
            get;
            private set;
        }

        public bool RequestRoomMembership(Room room) {
            if (Room != null) {
                return false;
            }

            Room = room;
            return true;
        }
    }
}