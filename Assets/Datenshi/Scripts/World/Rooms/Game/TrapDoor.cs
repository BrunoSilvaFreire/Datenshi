using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Doors {
    public class TrapDoor : Door {
        [SerializeField]
        private Spawner spawner;

        private void Start() {
            if (spawner == null) {
                spawner = Room.GetComponentInChildren<Spawner>();
            }

            if (spawner == null) {
                Destroy(this);
                return;
            }
            
            
        }
    }
}