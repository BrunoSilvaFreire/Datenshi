using System.Collections.Generic;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Datenshi.Scripts.World.Rooms.Doors {
    public class RequirementDoor : Door {
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
                    Check();
                });
            }
        }

        private void Check() {
            if (!deadRequired.IsEmpty()) {
                return;
            }

            Open();
        }
    }
}