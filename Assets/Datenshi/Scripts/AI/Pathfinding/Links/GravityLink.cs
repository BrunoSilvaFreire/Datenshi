using System;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Gravity;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Links {
    [Serializable]
    public class GravityLink : Link {
        public static readonly Color GizmosColor = Color.white;

        [SerializeField]
        private int destination;

        [SerializeField]
        private Vector2 requiredForce;


        public int Destination {
            get {
                return destination;
            }
        }

        private GravityPath loadedPath;

        public GravityLink(
            Navmesh navmesh,
            Vector2 initialPosition,
            Vector2 initialVelocity,
            float precision) {
            loadedPath = new GravityPath(initialPosition, initialVelocity, navmesh, precision);
            var end = loadedPath.FinalNode;
            if (end != null) {
                destination = navmesh.GetNodeIndex(end);
            } else {
                destination = -1;
            }
            requiredForce = initialVelocity;
        }

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
#if UNITY_EDITOR

        public override bool DrawOnlyOnMouseOver() {
            return true;
        }

        public override void DrawGizmos(Navmesh navmesh, uint originNodeIndex, float precision, bool precisionChanged) {
            if (precisionChanged || loadedPath == null) {
                var initPos = navmesh.WorldPosCenter(originNodeIndex);
                loadedPath = new GravityPath(initPos, RequiredForce, navmesh, precision);
            }
            var path = loadedPath.GetPath(navmesh, precision);
            Handles.color = GizmosColor;
            for (var i = 1; i < path.Length; i++) {
                var point = path[i];
                var previous = path[i - 1];

                Handles.DrawLine(previous, point);
            }
        }
#endif
    }
}