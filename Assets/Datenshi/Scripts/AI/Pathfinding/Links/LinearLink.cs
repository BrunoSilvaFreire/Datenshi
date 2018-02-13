using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Links {
    [Serializable]
    public class LinearLink : Link {
        public static readonly Color GizmosColor = new Color(1f, 0.92f, 0.23f);

        [SerializeField, ReadOnly]
        private uint destination;

        public LinearLink() { }

        public LinearLink(uint destination) {
            this.destination = destination;
        }

        public override void Execute() { }

        public override bool DrawOnlyOnMouseOver() {
            return false;
        }

        public override void DrawGizmos(Navmesh navmesh, uint originNodeIndex, float precision, bool precisionChanged) {
            var origin = navmesh.GetWorldPosition(originNodeIndex);
            var dest = navmesh.GetWorldPosition(destination);
            Handles.color = GizmosColor;
            Handles.DrawLine(origin, dest);
        }
    }
}