﻿using System;
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
        [ShowInInspector]
        private List<Vector2Int> path;

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
            /*
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
            );*/
            var grid = navmesh.Grid;
            var from = grid.WorldToCell(entityPos).ToVector2();
            var to = grid.WorldToCell(target).ToVector2();
            var job = new AerialPathfindingJob(from, to);
            var handle = job.Schedule();
            StartCoroutine(WaitForCompletion(job, handle));
        }

        private IEnumerator WaitForCompletion(AerialPathfindingJob job, JobHandle handle) {
            while (handle.IsCompleted) {
                yield return null;
            }
            LoadPath(job.Result);
        }

        private readonly List<BezierPoint> usedPoints = new List<BezierPoint>();

        private void LoadPath(NativeArray<Vector2Int> obj) {
            UpdatePoints(obj.Length);
            for (var i = 0; i < obj.Length; i++) {
                var node = obj[i];
                var point = usedPoints[i];
                point.position = node.ToFloat();
            }

            foreach (var point in usedPoints) {
                point.Revalidate();
            }
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

        public override void Execute(INavigable entity, DummyInputProvider provider) {
            if (path.IsNullOrEmpty()) {
                provider.Reset();
                return;
            }

            var entityPos = entity.Center;
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

            var dir = targetPos - entityPos;
            provider.Horizontal = dir.x;
            provider.Vertical = dir.y;
            Debug.DrawRay(entityPos, dir * 10, Color.white);
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
    }
}