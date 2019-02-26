using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.World.Rooms.Game {
    public class RoomAdvancedListener : MonoBehaviour, IRoomMember {
        [SerializeField]
        private UnityEvent onDestroyed;

        public UnityEvent OnDestroyed => onDestroyed;
        public EntityEvent OnEntityEnter;
        public UnityEvent OnPlayerEnter;

        public Room Room {
            get;
            private set;
        }

        private void Start() {
            Room.OnObjectEnter.AddListener(OnEntered);
        }

        private void OnEntered(Collider2D arg0) {
            var e = arg0.GetComponent<Entity>();
            if (e != null) {
                InvokeEntity(e);
            }
        }

        private void InvokeEntity(Entity entity) {
            OnEntityEnter.Invoke(entity);
            if (PlayerController.GetOrCreateEntity() == entity) {
                OnPlayerEnter.Invoke();
            }
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