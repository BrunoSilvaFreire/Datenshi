using System;
using System.Collections.Generic;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.Util;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Datenshi.Scripts.AI.Jobs {
    public struct AerialPathfindingJob : IJob {
        public const int MaxLength = 10;
        public readonly Vector2Int Start;
        public readonly Vector2Int End;
        private NativeArray<Vector2Int> result;

        public AerialPathfindingJob(Vector2Int start, Vector2Int end) {
            Start = start;
            End = end;
            result = new NativeArray<Vector2Int>(3, Allocator.TempJob);
        }

        public NativeArray<Vector2Int> Result => result;

        private static float Distance(Node from, Node to) {
            var a = from.Position;
            var b = to.Position;
            var x = Math.Abs(a.x - b.x);
            var y = Math.Abs(a.y - b.y);
            return Mathf.Sqrt(x * x + y * y);
        }

        public void Execute() {
            var navmesh = Navmesh.SilentInstance;
            if (navmesh == null) {
                Debug.LogError("Navmesh is null!");
                return;
            }

            var fromP = Start;
            var toP = End;
            var from = navmesh.GetNode(fromP);
            var to = navmesh.GetNode(toP);
            
            /*
            var worldMask = GameResources.Instance.WorldMask;

            if (!Physics2D.Linecast(fromP, toP, worldMask)) {
                result = new NativeArray<Vector2Int>(new[] {fromP, toP}, Allocator.TempJob);
                return;
            }*/


            if (from.IsBlocked || to.IsBlocked) {
                return;
            }

            // The set of nodes already evaluated.
            var closedSet = new HashSet<Node>();
            // The set of currently discovered nodes that are not evaluated yet.
            // Initially, only the start node is known.
            var openSet = new HashSet<Node> {
                from
            };
            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            var cameFrom = new Dictionary<Node, Node>();

            // For each node, the cost of getting from the start node to that node.
            var gScore = new Dictionary<Node, float> {
                [from] = 0.0f
            };

            // The cost of going from start to start is zero.

            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            var fScore = new Dictionary<Node, float> {
                [from] = Distance(from, to)
            };

            // For the first node, that value is completely heuristic.
            while (!openSet.IsEmpty()) {
                // the node in openSet having the lowest fScore[] value
                var current = openSet.MinBy(node => fScore.GetOrPut(node, () => float.PositiveInfinity));
                if (Equals(current, to)) {
                    ReconstructAerial(cameFrom, current);
                    return;
                }

                openSet.Remove(current);
                closedSet.Add(current);
                //Check all available neightboors
                var neightboors = new List<Node>();
                foreach (var direction in Direction.AllNonZero) {
                    if (navmesh.IsOutOfGridBounds(current.Position, direction)) {
                        continue;
                    }

                    var neightboor = navmesh.GetNeightboor(current, direction);
                    if (!closedSet.Contains(neightboor) && neightboor.IsEmpty) {
                        neightboors.Add(neightboor);
                    }
                }

                foreach (var neighboor in neightboors) {
                    if (!openSet.Contains(neighboor)) {
                        openSet.Add(neighboor);
                    }

                    //GScore = cost to get from start to current node, start always have 0
                    var currentGScore = gScore.GetOrPut(current, () => float.PositiveInfinity);
                    var neightborGScore = gScore.GetOrPut(neighboor, () => float.PositiveInfinity);
                    var linkDistance = Distance(current, neighboor);
                    // The distance from start to a neighbor
                    var tentativeGScore = currentGScore + linkDistance;
                    if (tentativeGScore >= neightborGScore) {
                        // This is not a better path.
                        continue;
                    }

                    // This path is the best until now. Record it!
                    cameFrom[neighboor] = current;
                    gScore[neighboor] = tentativeGScore;
                    //fScore is the "supposed lowest score"
                    fScore[neighboor] = gScore[neighboor] + Distance(neighboor, to);
                }
            }
        }

        private void ReconstructAerial(Dictionary<Node, Node> cameFrom, Node current) {
            // Here we "backtrack" the path defined by cameFrom, current starts as the destination
            if (!cameFrom.ContainsKey(current)) {
                return;
            }

            //result.CopyFrom();
            result = new NativeArray<Vector2Int>(cameFrom.Count, Allocator.Temp);
            result[0] = current.Position;
            const int currentIndex = 1;
            while (cameFrom.ContainsKey(current)) {
                var source = cameFrom[current];
                result[currentIndex] = source.Position;
                current = source;
            }
        }
    }
}