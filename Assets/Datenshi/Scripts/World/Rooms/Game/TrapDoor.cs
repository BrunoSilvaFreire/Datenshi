using System.Collections.Generic;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.World.Rooms.Game.Doors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Game {
    public class TrapDoor : Door {
        [SerializeField]
        private Spawner spawner;

        private bool closed;
        private bool spawnerOk;
        private bool roomOk;

        [ShowInInspector, ReadOnly]
        private List<ICombatant> deadRequired;

        private void Start() {
            deadRequired = new List<ICombatant>();
            foreach (var member in Room.Members) {
                var entity = member as ICombatant;
                if (entity == null) {
                    continue;
                }

                deadRequired.Add(entity);
                entity.OnKilled.AddListener(() => {
                    deadRequired.Remove(entity);
                    roomOk = deadRequired.IsEmpty();
                    CheckOpen();
                });
            }

            Open(true);
            Room.OnObjectEnter.AddListener(OnEnter);
        }


        private void OnCompleted() {
            spawnerOk = true;
            CheckOpen();
        }

        private void CheckOpen() {
            if (spawnerOk && roomOk) {
                Open();
            }
        }

        private void OnEnter(Collider2D arg0) {
            if (closed) {
                return;
            }

            var e = arg0.GetComponentInParent<Entity>();
            if (e != PlayerController.Instance.CurrentEntity) {
                return;
            }

            closed = true;
            Close();
            Room.OnObjectExit.RemoveListener(OnEnter);
            if (spawner != null) {
                spawner.OnWaveCompleted.AddListener(OnCompleted);                 
            } else {
                spawnerOk = true;
            }
        }
    }
}