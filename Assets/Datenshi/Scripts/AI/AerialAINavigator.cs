using System;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace Datenshi.Scripts.AI {
    [Serializable]
    public class SerializableNavigable : SerializableInterface<INavigable> { }

    public class AerialAINavigator : AINavigator {
        [ShowInInspector]
        private List<Vector2Int> path;


        [SerializeField, HideInInspector]
        private Object navigable;

        [ShowInInspector]
        public SerializableNavigable Navigable;

        public float MinimumHeightAdvantage = 3;

        [ShowInInspector, ReadOnly]
        private Navmesh navmesh;

        public float MinimumFavourableDistance = 10;

        private void Start() {
            navmesh = FindObjectOfType<Navmesh>();
        }

        protected override bool CanReload() {
            return true;
        }

        private void OnValidate() {
            if (navmesh == null) {
                navmesh = FindObjectOfType<Navmesh>();
            }
        }

        [Button]
        protected override void ReloadPath() {
            var entityPos = Navigable.Value.Center;
            if (navmesh.IsOutOfBounds(entityPos) || navmesh.IsOutOfBounds(Target)) {
                return;
            }

            var origin = navmesh.GetNodeAtWorld(entityPos);
            var target = navmesh.GetNodeAtWorld(Target);
            AStar.CalculatePathAerial(
                origin,
                target,
                navmesh,
                Navigable.ToString(),
                tempPath => { path = tempPath == null ? null : (from node in tempPath select node.Position).ToList(); }
            );
        }
#if UNITY_EDITOR

        private void OnDrawGizmos() {
            Gizmos.color = Color.cyan;
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
            var firstPos = navmesh.Grid.GetCellCenterWorld(first.ToVector3());
            var secondPos = navmesh.Grid.GetCellCenterWorld(second.ToVector3());
            Handles.Label(
                firstPos,
                string.Format("{0} {1}\n->\n{2} {3}", i, first, (i - 1), second));

            Gizmos.DrawLine(firstPos, secondPos);
        }
#endif

        public override void Execute(INavigable entity, AIStateInputProvider provider) {
            if (path == null) {
                return;
            }

            Vector2 dir;
            var entityPos = entity.Center;
            var currentNode = navmesh.GetNodeAtWorld(entityPos).Position;
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

        public override Vector2 GetFavourablePosition(ILocatable target) {
            var pos = Navigable.Value.Center;
            var targetPos = target.Center;
            var x = targetPos.x + Math.Sign(pos.x - targetPos.x) * MinimumFavourableDistance;
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