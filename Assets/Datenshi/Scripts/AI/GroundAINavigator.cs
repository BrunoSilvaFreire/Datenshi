using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.AI.Pathfinding.Links;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.AI {
    public class GroundAINavigator : AINavigator {
        [ShowInInspector, ReadOnly]
        private List<Link> path;

        [ShowInInspector, ReadOnly]
        private Link currentLink;

        public SerializableNavigable Navigable;
        public byte MaxTargetRepositionTries = 20;

        [ShowInInspector, ReadOnly]
        private Navmesh navmesh;

        private Vector2 target;

        public override Vector2 SetTarget(Vector2 t) {
            var node = navmesh.GetNodeAtWorld(t);
            if (!node.IsWalkable) {
                return target = FindWalkable(node);
            }

            return target = t;
        }

        private bool GetValidWalkable(Node node, Direction.DirectionValue dir, byte offset, out Node newNode) {
            var pos = node.Position;
            pos.y += dir * offset;
            newNode = navmesh.GetNode(pos);

            return newNode != null && newNode.IsWalkable;
        }

        private bool GetValidWalkable(Node node, byte offset, out Node newNode) {
            return GetValidWalkable(node, Direction.DirectionValue.Backward, offset, out newNode) ||
                   GetValidWalkable(node, Direction.DirectionValue.Foward, offset, out newNode);
        }

        private Vector2 FindWalkable(Node node) {
            for (byte currentTry = 1; currentTry <= MaxTargetRepositionTries; currentTry++) {
                Node result;
                if (GetValidWalkable(node, currentTry, out result)) {
                    return navmesh.WorldPosCenter(result);
                }
            }

            return Vector2.zero;
        }

        public override Vector2 GetTarget() {
            return target;
        }

        protected override bool CanReload() {
            return Navigable.Value.CollisionStatus.Down;
        }

        private void Start() {
            navmesh = FindObjectOfType<Navmesh>();
        }

        [Button]
        protected override void ReloadPath() {
            if (navmesh == null) {
                return;
            }

            var e = Navigable.Value;
            AStar.CalculatePath(
                navmesh.GetNodeAtWorld(e.GroundPosition),
                navmesh.GetNodeAtWorld(target),
                navmesh,
                e,
                p => {
                    path = p;
                    if (p != null) {
                        currentLink = p.Last();
                    }
                });
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Gizmos.DrawWireSphere(target, 1);
            if (path == null) {
                return;
            }

            foreach (var link in path) {
                Handles.color = link == currentLink ? Color.red : Color.cyan;
                link.DrawGizmos(navmesh, (uint) link.GetOrigin(), 10F, false);
            }
        }
#endif


        public override void Execute(INavigable entity, DummyInputProvider provider) {
            if (currentLink == null) {
                return;
            }

            if (Equals(navmesh.GetNodeAtWorld(entity.GroundPosition), navmesh.GetNode(currentLink.GetDestination()))) {
                if (path == null) {
                    currentLink = null;
                    return;
                }

                if (path.Count > 0) {
                    path.RemoveAt(path.Count - 1);
                }

                if (path.IsEmpty()) {
                    currentLink = null;
                    return;
                }

                currentLink = path.Last();
            }

            currentLink.Execute(entity, provider, navmesh);
        }

        public override Vector2 GetFavourablePosition(ILocatable target) {
            return target.Center;
        }

        public override Vector2 GetFavourablePosition(Vector2 targetPos) {
            return targetPos;
        }

        public override bool IsValid(Node node) {
            return node.IsWalkable;
        }
    }
}