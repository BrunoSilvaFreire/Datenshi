using System;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.AI.Pathfinding.Links;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.AI {
    public class GroundAINavigator : AINavigator {
        [ShowInInspector, ReadOnly]
        private List<Link> path;

        [ShowInInspector, ReadOnly]
        private Link currentLink;

        public INavigable Navigable;

        [ShowInInspector, ReadOnly]
        private Navmesh navmesh;

        protected override bool CanReload() {
            return Navigable.CollisionStatus.Down;
        }

        private void Start() {
            navmesh = FindObjectOfType<Navmesh>();
        }

        [Button]
        protected override void ReloadPath() {
            if (navmesh == null) {
                return;
            }

            AStar.CalculatePath(
                navmesh.GetNodeAtWorld(Navigable.GroundPosition),
                navmesh.GetNodeAtWorld(Target),
                navmesh,
                Navigable,
                p => {
                    path = p;
                    if (p != null) {
                        currentLink = p.Last();
                    }
                });
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Gizmos.DrawWireSphere(Target, 1);
            if (path == null) {
                return;
            }

            Gizmos.color = Color.magenta;
            foreach (var link in path) {
                link.DrawGizmos(navmesh, (uint) link.GetOrigin(), 10F, false);
            }
        }
#endif


        public override void Execute(INavigable entity, AIStateInputProvider provider) {
            if (currentLink == null) {
                return;
            }

            if (navmesh.GetNodeAtWorld(entity.GroundPosition) == navmesh.GetNode(currentLink.GetDestination())) {
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

        public override Vector2 GetFavourablePosition(INavigable target) {
            var pos = target.GroundPosition;
            /*
             TODO Fix
            if (target != null) {
                pos = target.GroundPosition;
            } else {
                pos = target.transform.position;
            }

            var state = s as RangedAttackStrategy;
            if (state != null) {
                pos.x += state.MinDistance * Math.Sign(INavigable.GroundPosition.x - pos.x);
            }

             */
            return pos;
        }
    }
}