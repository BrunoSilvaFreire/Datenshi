using System;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Links {
    [Serializable]
    public class LinearLink : Link {
        public static readonly Color GizmosColor = new Color(1f, 0.92f, 0.23f);

        [SerializeField, ReadOnly]
        private uint destination;

        [SerializeField, ReadOnly]
        private uint origin;

        public LinearLink() { }

        public LinearLink(uint destination, uint origin) {
            this.destination = destination;
            this.origin = origin;
        }

        public override int GetDestination() {
            return (int) destination;
        }

        public override int GetOrigin() {
            return (int) origin;
        }
#if UNITY_EDITOR
        
        public override bool DrawOnlyOnMouseOver() {
            return false;
        }

        public override void DrawGizmos(Navmesh navmesh, uint originNodeIndex, float precision, bool precisionChanged) {
            var origin = navmesh.GetWorldPosition(originNodeIndex);
            var dest = navmesh.GetWorldPosition(destination);
            Debug.DrawLine(origin, dest);
        }
#endif

        public override void Execute(MovableEntity entity, AIStateInputProvider provider, Navmesh navmesh) {
            provider.Horizontal = Math.Sign((int) destination - origin);
        }

        public override bool CanMakeIt(MovableEntity entity) {
            return true;
        }
    }
}