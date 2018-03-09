using System;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.AI.Pathfinding.Links;
using Datenshi.Scripts.Combat.Strategies;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.AI {
    public class GroundAIAgent : AIAgent {
        [ShowInInspector, ReadOnly]
        private List<Link> path;

        [ShowInInspector, ReadOnly]
        private Link currentLink;

        public MovableEntity Entity;
        public Navmesh Navmesh;

        protected override bool CanReload() {
            return Entity.CollisionStatus.Down;
        }

        [Button]
        protected override void ReloadPath() {
            path = AStar.CalculatePath(
                Navmesh.GetNodeAtWorld(Entity.GroundPosition),
                Navmesh.GetNodeAtWorld(Target),
                Navmesh,
                Entity);
            if (path != null) {
                currentLink = path.Last();
            }
        }


        private void OnDrawGizmos() {
            Gizmos.DrawWireSphere(Target, 1);
            if (path == null) {
                return;
            }

            Gizmos.color = Color.magenta;
            foreach (var link in path) {
                link.DrawGizmos(Navmesh, (uint) link.GetOrigin(), 10F, false);
            }
        }


        public override void Execute(MovableEntity entity, AIStateInputProvider provider) {
            if (currentLink == null) {
                return;
            }

            if (Navmesh.GetNodeAtWorld(entity.GroundPosition) == Navmesh.GetNode(currentLink.GetDestination())) {
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

            currentLink.Execute(entity, provider, Navmesh);
        }

        public override Vector2 GetFavourablePosition(RangedAttackStrategy state, LivingEntity target) {
            var movableEntity = target as MovableEntity;
            Vector2 pos;
            if (movableEntity != null) {
                pos = movableEntity.GroundPosition;
            } else {
                pos = target.transform.position;
            }
            pos.x += state.MinDistance * Math.Sign(Entity.GroundPosition.x - pos.x);
            return pos;
        }
    }
}