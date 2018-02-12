using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.AI.Pathfinding.Links.Generators;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Debugging {
    public class GravityTester : MonoBehaviour {
        public Vector2 Velocity;


        public float Gravity = GravityUtil.DefaultGravityForce;
        public float TimeIncrementation = GravityUtil.DefaultTimeIncrementation;
        public int XDivisions = 3;
        public int YDivisions = 3;
        public Navmesh Navmesh;
        public Vector2 BoxCastSize;
        private static bool loaded = false;
        [ShowInInspector, ReadOnly]
        private uint totalLinks;

        private static bool gizmos;
        private void OnValidate() {
            if (loaded) {
                return;
            }
            GravityUtil.OnCalculated += OnPathCalculated;
            loaded = true;
        }

        private static void OnPathCalculated(Vector2 vector2, Vector2 vector3, RaycastHit2D hit) {
            if (!gizmos) {
                return;
            }
            Gizmos.color = hit ? Color.red : Color.green;
            Gizmos.DrawLine(vector2, vector3);
        }

        private void OnDrawGizmos() {
            if (!Navmesh) {
                return;
            }
            gizmos = true;
            var pos = transform.position;
            var node = Navmesh.GetNodeAtWorld(pos);
            totalLinks = 0;
            foreach (var link in GravityLinkGenerator.DoGenerateLinks(node, Navmesh, Navmesh.WorldPosCenter(node), YDivisions, XDivisions, Velocity.x, Velocity.y, Gravity, TimeIncrementation, BoxCastSize)) {
                totalLinks++;
            }
            GravityUtil.CalculatePath(pos, Velocity, Gravity, Navmesh, BoxCastSize, out node, TimeIncrementation);
            gizmos = false;
        }
    }
}