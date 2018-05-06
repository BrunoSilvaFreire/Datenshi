using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UPM.Util;

namespace Datenshi.Scripts.AI.Pathfinding.Links.Generators {
    public sealed class GravityLinkGenerator : LinkGenerator {
        public int SpeedDivisions = 5;
        public int JumpDivisions = 5;
        public float Speed = 10F;
        public float Jump = 10F;
        public Vector2 ReferenceBounds;

        public override IEnumerable<Link> Generate(Node node, Navmesh navmesh, Vector2 nodeWorldPos, float precision) {
            if (!node.IsWalkable) {
                yield break;
            }

            var ySize = navmesh.Grid.cellSize.y / 2;
            var start = nodeWorldPos;
            start.y -= ySize - 0.1F;
            var links = DoGenerateLinks(
                node,
                navmesh,
                start,
                JumpDivisions,
                SpeedDivisions,
                Speed,
                Jump,
                precision);
            foreach (var generatedLink in links) {
                yield return generatedLink;
            }
        }

        public IEnumerable<Link> DoGenerateLinks(
            Node node,
            Navmesh navmesh,
            Vector2 nodeWorldPos,
            int jumpDivisions,
            int speedDivisions,
            float speed,
            float jump,
            float timeIncrementation) {
            for (var yi = 1; yi <= jumpDivisions; yi++) {
                for (var xi = 1; xi <= speedDivisions; xi++) {
                    var x = (float) xi / speedDivisions * speed;
                    var y = (float) yi / jumpDivisions * jump;
                    var direction = new Vector2(x, y);
                    GravityLink right;
                    if (DoTryGetLink(navmesh, node, nodeWorldPos, direction, timeIncrementation, out right)) {
                        yield return right;
                    }

                    direction.x = -direction.x;
                    GravityLink left;
                    if (DoTryGetLink(navmesh, node, nodeWorldPos, direction, timeIncrementation, out left)) {
                        yield return left;
                    }
                }
            }
        }
#if UNITY_EDITOR

        public override void DrawEditor() {
            SpeedDivisions = EditorGUILayout.IntField("SpeedDivisions", SpeedDivisions);
            JumpDivisions = EditorGUILayout.IntField("JumpDivisions", JumpDivisions);
            Speed = EditorGUILayout.FloatField("Speed", Speed);
            Jump = EditorGUILayout.FloatField("Jump", Jump);
            ReferenceBounds = EditorGUILayout.Vector2Field("Boxcast", ReferenceBounds);
        }
#endif


        public bool DoTryGetLink(
            Navmesh navmesh,
            Node node,
            Vector2 nodeWorldPos,
            Vector2 direction,
            float precision,
            out GravityLink link) {
            link = new GravityLink(navmesh.GetNodeIndex(node), navmesh, nodeWorldPos, direction, ReferenceBounds,
                precision);
            if (link.IsDefined && navmesh.GetNode(link.Destination).IsWalkable) {
                return !navmesh.IsOnSamePlatform(node, link.Destination);
            }

            return false;
        }
    }
}