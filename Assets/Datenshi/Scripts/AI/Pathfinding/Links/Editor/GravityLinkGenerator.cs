using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Links.Editor {
    public sealed class GravityLinkGenerator : LinkGenerator {
        public int SpeedDivisions = 5;
        public int JumpDivisions = 5;
        public float Speed = 10F;
        public float Jump = 10F;
        public float Gravity = Constants.DefaultGravity;
        public float TimeIncrementation = 0.05F;
        public Vector2 BoxcastSize;

        [ShowInInspector, UsedImplicitly]
        public BoxCollider2D CopyFrom {
            set {
                BoxcastSize = value.size;
            }
        }

        public override IEnumerable<Link> Generate(Node node, Navmesh navmesh, Vector2 nodeWorldPos) {
            for (var yi = 1; yi <= JumpDivisions; yi++) {
                for (var xi = 1; xi <= SpeedDivisions; xi++) {
                    var x = (float) xi / SpeedDivisions * Speed;
                    var y = (float) yi / JumpDivisions * Jump;
                    var direction = new Vector2(x, y);
                    GravityLink right;
                    if (TryGetLink(navmesh, node, nodeWorldPos, direction, out right)) {
                        yield return right;
                    }
                    direction.x = -direction.x;
                    GravityLink left;
                    if (TryGetLink(navmesh, node, nodeWorldPos, direction, out left)) {
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
            Gravity = EditorGUILayout.FloatField("Gravity", Gravity);
            TimeIncrementation =
                SirenixEditorFields.RangeFloatField("TimeIncrementation", TimeIncrementation, 0.001F, 1);
        }
#endif

        private bool TryGetLink(
            Navmesh navmesh,
            Node node,
            Vector2 nodeWorldPos,
            Vector2 direction,
            out GravityLink link) {
            link = new GravityLink(navmesh, nodeWorldPos, direction, Gravity, TimeIncrementation, BoxcastSize);
            if (link.IsDefined && navmesh.GetNode(link.Destination).IsWalkable) {
                return !navmesh.IsOnSamePlatform(node, link.Destination);
            }
            return false;
        }
    }
}