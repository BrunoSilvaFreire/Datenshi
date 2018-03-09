using System;
using System.Collections.Generic;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.AI.Pathfinding.Links;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class AStar {
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
            var cameFrom = new Dictionary<Node, AerialLink>();
            var gScore = new Dictionary<Node, float>();
            gScore[from] = 0.0f;
            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            var fScore = new Dictionary<Node, float>();

            fScore[from] = Distance(from, to, navMesh);
            var current = from;
            var boxSize = entity.Hitbox.bounds.size;
            var worldMask = GameResources.Instance.WorldMask;
            while (!openSet.IsEmpty()) {
                // the node in openSet having the lowest fScore[] value
                current = openSet.MinBy(node => fScore.GetOrPut(node, () => float.PositiveInfinity));
                if (current == to) {
                    return ReconstructAerial(cameFrom, current, navMesh);
                }
                var currentWorldPos = navMesh.WorldPosCenter(current);
                openSet.Remove(current);
                closedSet.Add(current);
                var neightboors = new List<Node>();
                foreach (var direction in Direction.AllNonZero) {
                    if (navMesh.IsOutOfGridBounds(current.Position, direction)) {
                        continue;
                    }
                    var neightboor = navMesh.GetNeightboor(current, direction);
                    if (neightboor.IsEmpty) {
                        neightboors.Add(neightboor);
                    }
                }
                foreach (var neighbor in neightboors) {
                    var neightboorWorldPos = navMesh.WorldPosCenter(neighbor.Position);
                    var dir = neightboorWorldPos - currentWorldPos;
                    if (Physics2D.BoxCast(neightboorWorldPos, boxSize, 0, dir, dir.magnitude, worldMask)) {
                        continue;
                    }

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
                    cameFrom[neighbor] = new AerialLink(navMesh.GetNodeIndex(current), navMesh.GetNodeIndex(neighbor));
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Distance(neighbor, to, navMesh);
                }
            }

            return null;
        }

        private static List<Node> ReconstructAerial(
            Dictionary<Node, AerialLink> cameFrom,
            Node current,
            Navmesh navMesh
        ) {
            if (!cameFrom.ContainsKey(current)) {
                return null;
            }

            var currentCameFrom = cameFrom[current];
            var totalPath = new List<Node> {
                navMesh.GetNode(currentCameFrom.From)
            };
            var lastX = 0;
            var lastY = 0;
            while (cameFrom.ContainsKey(current)) {
                AerialLink link;
                if (!cameFrom.TryGetValue(current, out link)) {
                    Debug.Log("Couldn find value for " + current + "@ " + cameFrom);
                    return totalPath;
                }

                var first = navMesh.GetWorldPosition((uint) link.From);
                var second = navMesh.GetWorldPosition((uint) link.To);
                Debug.DrawLine(
                    first,
                    second,
                    Color.yellow
                );
                var x = Math.Sign(second.x - first.x);
                var y = Math.Sign(second.y - first.y);
                if (x != lastX || y != lastY) {
                    lastX = x;
                    lastY = y;

                    totalPath.Add(current);
                }
                current = navMesh.GetNode(link.From);
            }
            if (!totalPath.Contains(current)) {
                totalPath.Add(current);
            }
            return totalPath;
        }
    }
}