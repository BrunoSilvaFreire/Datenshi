using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Links.Generators {
    public sealed class GravityLinkGenerator : LinkGenerator {
        public int SpeedDivisions = 5;
        public int JumpDivisions = 5;
        public float Speed = 10F;
        public float Jump = 10F;

        public override IEnumerable<Link> Generate(Node node, Navmesh navmesh, Vector2 nodeWorldPos, float precision) {
            if (!node.IsWalkable) {
                yield break;
            }
            var links = DoGenerateLinks(
                node,
                navmesh,
                nodeWorldPos,
                JumpDivisions,
                SpeedDivisions,
                Speed,
                Jump,
                precision,
                navmesh.BoxcastSize);
            foreach (var generatedLink in links) {
                yield return generatedLink;
            }
        }

        public static IEnumerable<Link> DoGenerateLinks(
            Node node,
            Navmesh navmesh,
            Vector2 nodeWorldPos,
            int jumpDivisions,
            int speedDivisions,
            float speed,
            float jump,
            float timeIncrementation,
            Vector2 boxcastSize) {
            for (var yi = 1; yi <= jumpDivisions; yi++) {
                for (var xi = 1; xi <= speedDivisions; xi++) {
                    var x = (float) xi / speedDivisions * speed;
                    var y = (float) yi / jumpDivisions * jump;
                    var direction = new Vector2(x, y);
                    GravityLink right;
                    if (DoTryGetLink(navmesh, node, nodeWorldPos, direction, timeIncrementation, boxcastSize, out right)) {
                        yield return right;
                    }
                    direction.x = -direction.x;
                    GravityLink left;
                    if (DoTryGetLink(navmesh, node, nodeWorldPos, direction, timeIncrementation, boxcastSize, out left)) {
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
        }
#endif


        public static bool DoTryGetLink(
            Navmesh navmesh,
            Node node,
            Vector2 nodeWorldPos,
            Vector2 direction,
            float precision,
            Vector2 boxcastSize,
            out GravityLink link) {
            link = new GravityLink(navmesh, nodeWorldPos, direction, precision);
            if (link.IsDefined && navmesh.GetNode(link.Destination).IsWalkable) {
                return !navmesh.IsOnSamePlatform(node, link.Destination);
            }
            return false;
        }
    }
}