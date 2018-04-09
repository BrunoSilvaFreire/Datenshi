using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.AI.Pathfinding.Links;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Misc;
using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class AStar {
        public static void CalculatePathAsync(Node @from,
            Node to,
            Navmesh navMesh,
            MovableEntity entity, Action<List<Node>> action) {
            new Thread(() => { action(CalculatePathAerial(from, to, navMesh, entity)); }).Start();
        }


        public static List<Link> CalculatePath(
            Node @from,
            Node to,
            Navmesh navMesh,
            MovableEntity entity) {
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
            var current = from;
            while (!openSet.IsEmpty()) {
                // the node in openSet having the lowest fScore[] value
                current = openSet.MinBy(node => fScore.GetOrPut(node, () => float.PositiveInfinity));
                if (current == to) {
                    return Reconstruct(cameFrom, current, navMesh);
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

            //Failure
            Debug.LogWarning("Couldn't find a path from " + from + " to " + to);
            return null;
        }

        private static float Distance(Node @from, Node to, Navmesh navMesh) {
            return Vector2.Distance(navMesh.WorldPosCenter(@from), navMesh.WorldPosCenter(to));
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

        public struct AerialLink {
            public int From;
            public int To;

            public AerialLink(int @from, int to) {
                this.From = @from;
                this.To = to;
            }
        }

        public static List<Node> CalculatePathAerial(Node from, Node to, Navmesh navMesh, MovableEntity entity) {
            if (from.IsBlocked || to.IsBlocked) {
                return null;
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


            var worldMask = GameResources.Instance.WorldMask;
            var fromP = navMesh.WorldPosCenter(from);
            var toP = navMesh.WorldPosCenter(to);
            if (!Physics2D.Linecast(fromP, toP, worldMask)) {
                return new List<Node> {
                    from,
                    to
                };
            }

            var boxSize = entity.Hitbox.bounds.size;
            while (!openSet.IsEmpty()) {
                // the node in openSet having the lowest fScore[] value
                var current = openSet.MinBy(node => fScore.GetOrPut(node, () => float.PositiveInfinity));
                if (current == to) {
                    return ReconstructAerial(cameFrom, current, navMesh, boxSize, worldMask);
                }

                var currentWorldPos = navMesh.WorldPosCenter(current);
                openSet.Remove(current);
                closedSet.Add(current);
                //Check all available neightboors
                var neightboors = new List<Node>();
                foreach (var direction in Direction.AllNonZero) {
                    if (navMesh.IsOutOfGridBounds(current.Position, direction)) {
                        continue;
                    }

                    if (Physics2D.BoxCast(currentWorldPos, boxSize, 0, direction, 1, worldMask)) {
                        continue;
                    }

                    var neightboor = navMesh.GetNeightboor(current, direction);
                    if (!closedSet.Contains(neightboor) && neightboor.IsEmpty) {
                        neightboors.Add(neightboor);
                    }
                }

                foreach (var neighboor in neightboors) {
                    var neightboorWorldPos = navMesh.WorldPosCenter(neighboor.Position);
                    var dir = neightboorWorldPos - currentWorldPos;
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

            return null;
        }

        private static List<Node> ReconstructAerial(
            Dictionary<Node, Node> cameFrom,
            Node current,
            Navmesh navmesh,
            Vector2 size,
            LayerMask worldMask
        ) {
            // Here we "backtrack" the path defined by cameFrom, current starts as the destination
            if (!cameFrom.ContainsKey(current)) {
                return null;
            }

            var totalPath = new List<Node> {
                current
            };
            var lastNoHit = current;
            while (cameFrom.ContainsKey(current)) {
                var source = cameFrom[current];
                var firstPos = navmesh.WorldPosCenter(lastNoHit);
                var secondPos = navmesh.WorldPosCenter(source);
                var dir = secondPos - firstPos;
                if (Physics2D.BoxCast(firstPos, size, 0, dir, dir.magnitude, worldMask)) {
                    lastNoHit = current;
                    totalPath.Add(current);
                }

                current = source;
            }

            totalPath.Add(current);
            return totalPath;
        }
    }
}