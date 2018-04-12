using System;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Util.Gravity;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Links {
    [Serializable]
    public class GravityLink : Link {
        public static readonly Color GizmosColor = Color.white;

        [SerializeField, ReadOnly]
        private int destination;

        [SerializeField, ReadOnly]
        private int origin;

        [SerializeField]
        private Vector2 requiredForce;


        public int Destination {
            get {
                return destination;
            }
        }

        private GravityPath loadedPath;

        public GravityLink(
            int origin,
            Navmesh navmesh,
            Vector2 initialPosition,
            Vector2 initialVelocity,
            float precision) {
            this.origin = origin;
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

        public override int GetDestination() {
            return destination;
        }

        public override int GetOrigin() {
            return origin;
        }

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

        public override void Execute(MovableEntity entity, AIStateInputProvider provider, Navmesh navmesh) {
            var pos = entity.GroundPosition;
            var direction = Math.Sign(requiredForce.x);
            var originPos = navmesh.WorldPosCenter((uint) origin);
            var distance = Mathf.Abs(Mathf.Abs(originPos.x) - Mathf.Abs(pos.x));
            if (distance > 0.5F) {
                //Run towards middle
                provider.Jump = false;
                provider.Horizontal = direction;
            } else if (entity.CollisionStatus.Down) {
                entity.Velocity = requiredForce;
            }
        }

        public override bool CanMakeIt(MovableEntity entity) {
            return entity.MaxSpeed > requiredForce.x && entity.YForce > requiredForce.y;
        }
    }
}