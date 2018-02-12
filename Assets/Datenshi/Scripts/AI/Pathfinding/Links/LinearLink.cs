using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Links {
    public class LinearLink : Link {
        public static readonly Color GizmosColor = new Color(0.46f, 1f, 0.01f);

        [SerializeField, ReadOnly]
        private uint first;

        [SerializeField, ReadOnly]
        private uint second;

        public LinearLink() { }

        public LinearLink(uint first, uint second) {
            this.first = first;
            this.second = second;
        }

        public override void Execute() { }

        public override void DrawGizmos(Navmesh navmesh) {
            var pos1 = navmesh.GetWorldPosition(first);
            var pos2 = navmesh.GetWorldPosition(second);
            Gizmos.color = GizmosColor;
            Gizmos.DrawLine(pos1, pos2);
        }
    }
}