using System;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Links {
    [Serializable]
    public class GravityLink : Link {
        public static readonly Color GizmosColor = new Color(0.96f, 0f, 0.34f);

        [SerializeField]
        private int destination;

        public int Destination {
            get {
                return destination;
            }
        }

        public GravityLink(
            Navmesh navmesh,
            Vector2 nodeWorldPos,
            Vector2 direction,
            float gravity,
            float timeIncrementation,
            Vector2 boxcastSize) {
            Node end;
            this.path = GravityUtil.CalculatePath(
                    nodeWorldPos,
                    direction,
                    timeIncrementation,
                    gravity,
                    navmesh,
                    boxcastSize,
                    out end)
                .ToArray();
            //fromNode = navmesh.GetNodeIndex(navmesh.GetNodeAtWorld(nodeWorldPos));
            if (end != null) {
                destination = navmesh.GetNodeIndex(end);
            } else {
                destination = -1;
            }
            requiredForce = direction;
        }

        [SerializeField]
        private Vector2 requiredForce;

        [SerializeField]
        private Vector2[] path;

        public Vector2 RequiredForce {
            get {
                return requiredForce;
            }
        }

        public bool IsDefined {
            get {
                return destination > 0;
            }
        }

        public override void Execute() { }

        public override void DrawGizmos(Navmesh navmesh) {
            GravityUtil.DrawDebug(path, GizmosColor);
        }
    }
}