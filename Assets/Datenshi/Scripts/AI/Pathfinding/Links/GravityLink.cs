using System;
using Datenshi.Scripts.Combat.Gravity;
using Datenshi.Scripts.Movement.Config;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Links {
    [Serializable]
    public class GravityLink : Link {
        [SerializeField, ReadOnly]
        private int destination;

        [SerializeField, ReadOnly]
        private int origin;

        [SerializeField]
        private Vector2 requiredForce;

        private Vector2 colliderSize;

        public int Destination => destination;

        private GravityPath loadedPath;

        public GravityLink(
            int origin,
            Navmesh navmesh,
            Vector2 initialPosition,
            Vector2 initialVelocity,
            Vector2 boxcastSize,
            float precision) {
            this.origin = origin;
            colliderSize = boxcastSize;
            loadedPath = new GravityPath(initialPosition, initialVelocity, boxcastSize, navmesh, precision);
            var end = loadedPath.FinalNode;
            if (end != null) {
                destination = navmesh.GetNodeIndex(end);
            } else {
                destination = -1;
            }

            requiredForce = initialVelocity;
        }

        public Vector2 RequiredForce => requiredForce;

        public bool IsDefined => destination > 0;

        public override int GetDestination() {
            return destination;
        }

        public override bool CanMakeIt(INavigable entity) {
            var m = entity.MovementConfig;
            var b = true;
            if (m != null) {
                b = m.VerticalForce > requiredForce.y;
            }

            return m.MaxSpeed > requiredForce.x && b;
        }

        private bool CanFit(INavigable entity) {
            var size = entity.Hitbox.bounds.size;
            return size.x < colliderSize.x && size.y < colliderSize.y;
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
                loadedPath = new GravityPath(initPos, RequiredForce, colliderSize, navmesh, precision);
            }

            var path = loadedPath.GetPath(navmesh, precision);
            for (var i = 1; i < path.Length; i++) {
                var point = path[i];
                var previous = path[i - 1];

                Handles.DrawLine(previous, point);
            }
        }
#endif

        public override void Execute(INavigable entity, DummyInputProvider provider, Navmesh navmesh) {
            var pos = entity.GroundPosition;
            var direction = Math.Sign(requiredForce.x);
            var originPos = navmesh.WorldPosCenter((uint) origin);
            var distance = Mathf.Abs(Mathf.Abs(originPos.x) - Mathf.Abs(pos.x));
            if (distance > 0.5F) {
                //Run towards middle
                provider.Jump.Consume();
                provider.Horizontal = direction;
            } else if (entity.CollisionStatus.Down) {
                entity.Velocity = requiredForce;
                provider.Reset();
            }
        }
    }
}