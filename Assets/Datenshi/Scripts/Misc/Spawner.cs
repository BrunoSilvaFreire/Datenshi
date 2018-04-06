using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Interaction;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class Spawner : InteractableElement {
        public GameObject Prefab;
        public Transform Center;
        public float Radius;

        public override bool CanInteract(MovableEntity e) {
            return true;
        }

        protected override void Execute(MovableEntity e) {
            var pos = Center.position;
            pos.x += Random.value * Radius * (Random.value > 0.5 ? 1 : -1);
            Instantiate(Prefab, pos, Quaternion.identity);
        }
    }
}