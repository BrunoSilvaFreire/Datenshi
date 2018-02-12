using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Links {
    public class LinearLink : Link {
        public static readonly Color GizmosColor = new Color(1f, 0.92f, 0.23f);

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
            //HandlesUtil.ArrowDebug(pos1, pos2, GizmosColor);
            Debug.DrawLine(pos1, pos2, GizmosColor);
        }
    }
}