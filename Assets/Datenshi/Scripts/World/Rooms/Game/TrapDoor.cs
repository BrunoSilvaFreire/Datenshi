using Datenshi.Scripts.World.Rooms.Doors;
using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Game {
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
            Open(true);
            
        }
    }
}