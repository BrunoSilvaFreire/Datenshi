using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BezierSolution;
using Datenshi.Scripts.AI.Jobs;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.AI.Util;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Jobs;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace Datenshi.Scripts.AI {
    [Serializable]
    public class SerializableNavigable : SerializableInterface<INavigable> { }

    public class AerialAINavigator : AINavigator {
        public BezierPointPool Pool;
        public BezierSpline Spline;

        [ShowInInspector]
        public SerializableNavigable Navigable;

        public float MinimumHeightAdvantage = 3;

        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
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

        private Vector2 target;

        public override Vector2 SetTarget(Vector2 target) {
            return this.target = target;
        }

        public override Vector2 GetTarget() {
            return target;
        }

        [Button]
        protected override void ReloadPath() {
            var entityPos = Navigable.Value.Center;

            if (navmesh.IsOutOfBounds(entityPos) || navmesh.IsOutOfBounds(target)) {
                return;
            }

            var origin = navmesh.GetNodeAtWorld(entityPos);
            var t = navmesh.GetNodeAtWorld(target);
            AStar.CalculatePathAerial(
                origin,
                t,
                navmesh,
                Navigable.ToString(),
                LoadPath
            );

            /*var grid = navmesh.Grid;
            var from = grid.WorldToCell(entityPos).ToVector2();
            var to = grid.WorldToCell(target).ToVector2();
            var job = new AerialPathfindingJob(from, to);
            var handle = job.Schedule();
            StartCoroutine(WaitForCompletion(job, handle));
            */
        }
/*
    private IEnumerator WaitForCompletion(AerialPathfindingJob job, JobHandle handle) {
        while (handle.IsCompleted) {
            yield return null;
        }

        handle.Complete();
        LoadPath(job.Result);
    }*/

        private readonly List<BezierPoint> usedPoints = new List<BezierPoint>();

        private List<Vector2Int> path;
        //private List<Node> toUpdate;

        private void LoadPath(List<Node> obj) {
            if (obj == null) {
                return;
            }

            var list = new List<Vector2Int>();
            foreach (var node in obj) {
                if (node == null) {
                    continue;
                }

                list.Add(node.Position);
            }

            path = list;
        }

        private void UpdatePoints(int needed) {
            var inUse = usedPoints.Count;
            if (needed > inUse) {
                AllocatePoints(needed - inUse);
            } else if (inUse > needed) {
                DelocatePoints(inUse - needed);
            }

            //We have exactly enough
        }

        private void AllocatePoints(int i) {
            for (var j = 0; j < i; j++) {
                usedPoints.Add(Pool.Get());
            }
        }

        private void DelocatePoints(int i) {
            for (var j = 0; j < i; j++) {
                var point = usedPoints[j];
                usedPoints.RemoveAt(j);
                Pool.Return(point);
            }
        }
#if UNITY_EDITOR

        private void OnDrawGizmos() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(target, 1);
            if (usedPoints == null) {
                return;
            }

            var grid = navmesh.Grid;
            for (var i = 1; i < usedPoints.Count; i++) {
                var first = usedPoints[i];
                var second = usedPoints[i - 1];
                var a = grid.WorldToCell(first.position);
                var b = grid.WorldToCell(second.position);
                Draw(a, b, i);
            }
        }

        private void Draw(Vector3Int first, Vector3Int second, int i) {
            var firstPos = navmesh.Grid.GetCellCenterWorld(first);
            var secondPos = navmesh.Grid.GetCellCenterWorld(second);
            Handles.Label(
                firstPos,
                $"{i} {first}\n->\n{(i - 1)} {second}");

            Gizmos.DrawLine(firstPos, secondPos);
        }
#endif

        public override void Execute(INavigable entity, DummyInputProvider provider) {
            //TestUpdate();

            Vector2 dir;
            var entityPos = entity.Center;
            if (path.IsNullOrEmpty()) {
                dir = target - entityPos;
            } else {
                var currentNode = navmesh.GetNodeAtWorld(entityPos).Position;
                int lastIndex;
                if (path.Count == 1) {
                    lastIndex = 0;
                } else {
                    lastIndex = path.Count - 1;
                }

                var targetNode = path[lastIndex];
                var targetPos = (Vector2) navmesh.Grid.GetCellCenterWorld(targetNode.ToVector3());
                if (currentNode == targetNode) {
                    path.RemoveAt(lastIndex);
                    if (path.IsEmpty()) {
                        provider.Reset();
                        return;
                    }

                    targetNode = path[path.Count - 1];
                    targetPos = navmesh.Grid.GetCellCenterWorld(targetNode.ToVector3());
                }

                dir = targetPos - entityPos;
            }

            dir.Normalize();
            provider.Horizontal = dir.x;
            provider.Vertical = dir.y;
            Debug.DrawRay(entityPos, dir * 10, Color.white);
        }
/*

        private void TestUpdate() {
            if (toUpdate == null) {
                return;
            }

            UpdatePoints(toUpdate.Count);
            var total = toUpdate.Count;
            for (var i = 0; i < total; i++) {
                var node = toUpdate[i];
                var point = usedPoints[i];
                point.position = node.Position.ToFloat() + new Vector2(.5F, .5F);
                if (i > 0) {
                    point.precedingControlPointPosition = toUpdate[i - 1].Position.ToFloat() + new Vector2(.5F, .5F);
                } else {
                    point.precedingControlPointLocalPosition = Vector2.zero;
                }

                if (i < total - 1) {
                    point.followingControlPointPosition = toUpdate[i + 1].Position.ToFloat() + new Vector2(.5F, .5F);
                } else {
                    point.followingControlPointLocalPosition = Vector2.zero;
                }

                point.transform.parent = Spline.transform;
            }

            toUpdate = null;
        }*/

        public override Vector2 GetFavourableStartPosition(INavigable navigable) {
            return navigable.Center;
        }

        public override Vector2 GetFavourablePosition(ILocatable target) {
            return GetFavourablePosition(target.Center);
        }

        public override Vector2 GetFavourablePosition(Vector2 targetPos) {
            var pos = Navigable.Value.Center;
            var x = targetPos.x + Math.Sign(pos.x - targetPos.x) * MinimumFavourableDistance;
            var y = targetPos.y + MinimumHeightAdvantage;
            return new Vector2(x, y);
        }

        public override bool IsValid(Node node) {
            return node.IsEmpty;
        }
    }
}