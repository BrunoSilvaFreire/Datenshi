using System;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.Combat.Strategies;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.AI {
    public class AerialPath { }

    public class AerialAIAgent : AIAgent {
        [ShowInInspector]
        private List<Vector2Int> path;

        public MovableEntity Entity;
        public float MinimumHeightAdvantage = 3;
        public Navmesh Navmesh;

        protected override bool CanReload() {
            return true;
        }

        [Button]
        protected override void ReloadPath() {
            var entityPos = Entity.transform.position;
            if (Navmesh.IsOutOfBounds(entityPos) || Navmesh.IsOutOfBounds(Target)) {
                return;
            }

            var origin = Navmesh.GetNodeAtWorld(entityPos);
            var target = Navmesh.GetNodeAtWorld(Target);
            var tempPath = AStar.CalculatePathAerial(origin, target, Navmesh, Entity);
            path = tempPath == null ? null : (from node in tempPath select node.Position).ToList();
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Target, 1);
            if (path == null) {
                return;
            }

            for (var i = 1; i < path.Count; i++) {
                var first = path[i];
                var second = path[i - 1];
                Draw(first, second, i);
            }
        }

        private void Draw(Vector2Int first, Vector2Int second, int i) {
            var firstPos = Navmesh.Grid.GetCellCenterWorld(first.ToVector3());
            var secondPos = Navmesh.Grid.GetCellCenterWorld(second.ToVector3());
            Handles.Label(
                firstPos,
                string.Format("{0} {1}\n->\n{2} {3}", i, first, (i - 1), second));

            Gizmos.DrawLine(firstPos, secondPos);
        }

        public override void Execute(MovableEntity entity, AIStateInputProvider provider) {
            if (path == null) {
                return;
            }

            Vector2 dir;
            var entityPos = entity.transform.position;
            var currentNode = Navmesh.GetNodeAtWorld(entityPos).Position;
            var lastIndex = path.Count - 1;
            var targetNode = path[lastIndex];
            if (path.Count < 2) {
                // Is not between last 2 points
                dir = targetNode - currentNode;
            } else {
                if (currentNode == targetNode) {
                    path.RemoveAt(lastIndex);
                    targetNode = path[path.Count - 1];
                }

                dir = targetNode - currentNode;
                dir.Normalize();
            }

            provider.Horizontal = dir.x;
            provider.Vertical = dir.y;
            return;
        }

        public override Vector2 GetFavourablePosition(RangedAttackStrategy state, LivingEntity target) {
            var pos = Entity.transform.position;
            var targetPos = target.transform.position;
            var x = targetPos.x + Math.Sign(pos.x - targetPos.x) *
                    Entity.DefaultAttackStrategy.GetMinimumDistance(Entity, target);
            float y;
            if (pos.y - targetPos.y > MinimumHeightAdvantage) {
                y = targetPos.y + MinimumHeightAdvantage;
            } else {
                y = targetPos.y + MinimumHeightAdvantage;
            }

            return new Vector2(x, y);
        }
    }
}