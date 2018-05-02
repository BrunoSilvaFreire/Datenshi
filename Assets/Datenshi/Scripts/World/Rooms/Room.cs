using System.Collections.Generic;
using Datenshi.Scripts.Util;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.World.Rooms {
    public interface IRoomMember {
        UnityEvent OnDestroyed {
            get;
        }
    }

    public class Room : MonoBehaviour {
        private List<IRoomMember> entities = new List<IRoomMember>();
        public Vector2 Size;

        public bool IsInBounds(Vector2 pos) {
            return !IsOutInBounds(pos);
        }

        public bool IsOutInBounds(Vector2 pos) {
            var center = transform.position;
            var halfWidth = Size.x / 2;
            var halfHeigth = Size.y / 2;
            var minX = center.x - halfWidth;
            var maxX = center.x + halfWidth;
            var minY = center.y - halfHeigth;
            var maxY = center.y + halfHeigth;
            var posX = pos.x;
            var posY = pos.y;
            return posX > maxX || posX < minX || posY > maxY || posY < minY;
        }

        private void OnDrawGizmos() {
            GizmosUtil.DrawBox2DWire(transform.position, Size, Color.magenta);
        }

        public void RegisterEntity(IRoomMember entity) {
            if (entities.Contains(entity)) {
                return;
            }

            entities.Add(entity);
            var e = entity.OnDestroyed;
            UnityAction action = null;
            action = () => {
                entities.Remove(entity);
                if (action != null) {
                    e.RemoveListener(action);
                }
            };
            e.AddListener(action);
        }
    }
}