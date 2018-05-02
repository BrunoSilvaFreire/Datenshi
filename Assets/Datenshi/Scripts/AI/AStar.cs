using System;
using System.Collections.Generic;
using System.Threading;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.AI.Pathfinding.Links;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using UnityEngine;
using UnityEngine.Profiling;

namespace Datenshi.Scripts.AI {
    public static class AStar {
        public static void CalculatePath(
            Node @from,
            Node to,
            Navmesh navMesh,
            INavigable entity,
            Action<List<Link>> action) {
            ThreadPool.QueueUserWorkItem(
                state => {
                    // The set of nodes already evaluated.
                    var closedSet = new List<Node>();
                    // The set of currently discovered nodes that are not evaluated yet.
                    // Initially, only the start node is known.
                    var openSet = new List<Node> {
                        from
                    };
                    // For each node, which node it can most efficiently be reached from.
                    // If a node can be reached from many nodes, cameFrom will eventually contain the
                    // most efficient previous step.
                    var cameFrom = new Dictionary<Node, Link>();
                    var gScore = new Dictionary<Node, float>();
                    gScore[from] = 0.0f;
                    // For each node, the total cost of getting from the start node to the goal
                    // by passing by that node. That value is partly known, partly heuristic.
                    var fScore = new Dictionary<Node, float>();

                    fScore[from] = Distance(from, to, navMesh);
                    while (!openSet.IsEmpty()) {
                        // the node in openSet having the lowest fScore[] value
                        var current = openSet.MinBy(node => fScore.GetOrPut(node, () => float.PositiveInfinity));
                        if (Equals(current, to)) {
                            action(Reconstruct(cameFrom, current, navMesh));
                            return;
                        }

                        openSet.Remove(current);
                        closedSet.Add(current);
                        foreach (var link in current.Links) {
                            if (!link.CanMakeIt(entity)) {
                                continue;
                            }

                            var neighbor = navMesh.GetNode(link.GetDestination());
                            if (closedSet.Contains(neighbor)) {
                                // Ignore the neighbor which is already evaluated.
                                continue;
                            }

                            var currentGScore = gScore.GetOrPut(current, () => float.PositiveInfinity);
                            var neightborGScore = gScore.GetOrPut(neighbor, () => float.PositiveInfinity);
                            var linkDistance = Distance(current, neighbor, navMesh);
                            // The distance from start to a neighbor
                            var tentativeGScore = currentGScore + linkDistance;
                            if (!openSet.Contains(neighbor)) {
                                openSet.Add(neighbor);
                            } else if (tentativeGScore >= neightborGScore) {
                                // This is not a better path.
                                continue;
                            }

                            // This path is the best until now. Record it!
                            cameFrom[neighbor] = link;
                            gScore[neighbor] = tentativeGScore;
                            fScore[neighbor] = gScore[neighbor] + Distance(neighbor, to, navMesh);
                        }
                    }

                    action(null);
                    //Failure
                });
        }

        private static float Distance(Node from, Node to, Navmesh navMesh) {
            var a = from.Position;
            var b = to.Position;
            var x = Math.Abs(a.x - b.x);
            var y = Math.Abs(a.y - b.y);
            return Mathf.Sqrt(x * x + y * y);
        }

        private static List<Link> Reconstruct(IDictionary<Node, Link> cameFrom, Node current, Navmesh navmesh) {
            if (!cameFrom.ContainsKey(current)) {
                return null;
            }

            var currentCameFrom = cameFrom[current];
            var totalPath = new List<Link> {
                currentCameFrom
            };
            while (cameFrom.ContainsKey(current)) {
                Link link;
                if (!cameFrom.TryGetValue(current, out link)) {
                    Debug.Log("Couldn find value for " + current + "@ " + cameFrom);
                    return totalPath;
                }

                ;
                current = navmesh.GetNode(link.GetOrigin());
                totalPath.Add(link);
            }

            return totalPath;
        }


        public static void CalculatePathAerial(
            Node from,
            Node to,
            Navmesh navMesh,
            string entity,
            Action<List<Node>> action) {
            var fromP = navMesh.WorldPosCenter(from);
            var toP = navMesh.WorldPosCenter(to);
            var worldMask = GameResources.Instance.WorldMask;

            if (!Physics2D.Linecast(fromP, toP, worldMask)) {
                action(
                    new List<Node> {
                        from,
                        to
                    });
                return;
            }

            ThreadPool.QueueUserWorkItem(
                state => {
                    if (from.IsBlocked || to.IsBlocked) {
                        action(null);
                        return;
                    }

                    // The set of nodes already evaluated.
                    var closedSet = new List<Node>();
                    // The set of currently discovered nodes that are not evaluated yet.
                    // Initially, only the start node is known.
                    var openSet = new List<Node> {
                        from
                    };
                    // For each node, which node it can most efficiently be reached from.
                    // If a node can be reached from many nodes, cameFrom will eventually contain the
                    // most efficient previous step.
                    var cameFrom = new Dictionary<Node, Node>();

                    // For each node, the cost of getting from the start node to that node.
                    var gScore = new Dictionary<Node, float>();

                    // The cost of going from start to start is zero.
                    gScore[from] = 0.0f;

                    // For each node, the total cost of getting from the start node to the goal
                    // by passing by that node. That value is partly known, partly heuristic.
                    var fScore = new Dictionary<Node, float>();

                    // For the first node, that value is completely heuristic.
                    fScore[from] = Distance(from, to, navMesh);


                    while (!openSet.IsEmpty()) {
                        // the node in openSet having the lowest fScore[] value
                        var current = openSet.MinBy(node => fScore.GetOrPut(node, () => float.PositiveInfinity));
                        if (Equals(current, to)) {
                            action(ReconstructAerial(cameFrom, current));
                            Profiler.EndThreadProfiling();
                            return;
                        }

                        openSet.Remove(current);
                        closedSet.Add(current);
                        //Check all available neightboors
                        var neightboors = new List<Node>();
                        foreach (var direction in Direction.AllNonZero) {
                            if (navMesh.IsOutOfGridBounds(current.Position, direction)) {
                                continue;
                            }

                            var neightboor = navMesh.GetNeightboor(current, direction);
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
                            var linkDistance = Distance(current, neighboor, navMesh);
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
                            fScore[neighboor] = gScore[neighboor] + Distance(neighboor, to, navMesh);
                        }
                    }

                    action(null);
                });
        }

        private static List<Node> ReconstructAerial(
            Dictionary<Node, Node> cameFrom,
            Node current
        ) {
            // Here we "backtrack" the path defined by cameFrom, current starts as the destination
            if (!cameFrom.ContainsKey(current)) {
                return null;
            }

            var totalPath = new List<Node> {
                current
            };
            while (cameFrom.ContainsKey(current)) {
                var source = cameFrom[current];
                current = source;
            }

            totalPath.Add(current);
            return totalPath;
        }
    }
}