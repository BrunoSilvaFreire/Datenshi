using System;
using System.Collections.Generic;
using Datenshi.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.World.Rooms {
    public interface IRoomMember {
        UnityEvent OnDestroyed {
            get;
        }

        Room Room {
            get;
        }

        bool RequestRoomMembership(Room room);
    }

    [Serializable]
    public class RoomCollisionEvent : UnityEvent<Collider2D> { }

    public class Room : MonoBehaviour {
        private readonly List<IRoomMember> members = new List<IRoomMember>();
        public BoxCollider2D Area;
        public IEnumerable<IRoomMember> Members => members;

        public float Width => Area.size.x;
        public float Height => Area.size.y;
        public RoomCollisionEvent OnObjectEnter;
        public RoomCollisionEvent OnObjectExit;

        public bool IsInBounds(Vector2 pos) {
            return !IsOutInBounds(pos);
        }

        public bool IsOutInBounds(Vector2 pos) {
            var size = Area.bounds.size;
            var center = transform.position;
            var halfWidth = size.x / 2;
            var halfHeigth = size.y / 2;
            var minX = center.x - halfWidth;
            var maxX = center.x + halfWidth;
            var minY = center.y - halfHeigth;
            var maxY = center.y + halfHeigth;
            var posX = pos.x;
            var posY = pos.y;
            return posX > maxX || posX < minX || posY > maxY || posY < minY;
        }

        private void Awake() {
            foreach (var member in GetComponentsInChildren<IRoomMember>()) {
                if (members.Contains(member)) {
                    continue;
                }

                member.RequestRoomMembership(this);
                members.Add(member);
                var e = member.OnDestroyed;
                UnityAction action = null;
                action = () => {
                    members.Remove(member);
                    if (action != null) {
                        e.RemoveListener(action);
                    }
                };
                e.AddListener(action);
            }
        }

        private void OnDrawGizmos() {
            if (Area == null) {
                return;
            }

            GizmosUtil.DrawBounds2D(Area.bounds, Color.magenta);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            OnObjectEnter.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other) {
            OnObjectExit.Invoke(other);
        }
    }
}