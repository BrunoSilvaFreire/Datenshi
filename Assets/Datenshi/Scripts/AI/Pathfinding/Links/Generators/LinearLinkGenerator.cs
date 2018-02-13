﻿using System;
using System.Collections.Generic;
using Datenshi.Scripts.Misc;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Links.Generators {
    [Serializable]
    public sealed class LinearLinkGenerator : LinkGenerator {
        public override IEnumerable<Link> Generate(Node node, Navmesh navmesh, Vector2 nodeWorldPos, float precision) {
            if (!node.IsWalkable) {
                yield break;
            }
            Link left;
            if (TryGetLink(node, navmesh, Direction.Left, out left)) {
                yield return left;
            }
            Link right;
            if (TryGetLink(node, navmesh, Direction.Right, out right)) {
                yield return right;
            }
        }


        private static bool TryGetLink(Node node, Navmesh navmesh, Direction direction, out Link link) {
            link = default(Link);
            if (navmesh.IsOutOfGridBounds(node.Position, direction)) {
                return false;
            }
            var neightboor = navmesh.GetNeightboor(node, direction);
            if (neightboor.IsInvalid || !neightboor.IsWalkable) {
                return false;
            }
            var found = navmesh.GetNodeIndex(neightboor);
            if (found < 0) {
                return false;
            }
            link = new LinearLink((uint) found);
            return true;
        }


#if UNITY_EDITOR
        //Not needed
        public override void DrawEditor() { }
#endif
    }
}