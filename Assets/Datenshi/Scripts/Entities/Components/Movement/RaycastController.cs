using Datenshi.Scripts.Controller;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Components.Movement {
    [RequireComponent(typeof(BoxCollider2D))]
    public class RaycastController : MonoBehaviour {
        [ShowInInspector]
        protected LayerMask CollisionMask;

        public const float SkinWidth = .015f;
        private const float DstBetweenRays = .25f;

        [HideInInspector]
        public int HorizontalRayCount;

        [HideInInspector]
        public int VerticalRayCount;

        [HideInInspector]
        public float HorizontalRaySpacing;

        [HideInInspector]
        public float VerticalRaySpacing;

        [ReadOnly]
        public BoxCollider2D Coll;

        [HideInInspector]
        public RaycastOrigins Origins;

        public virtual void Awake() {
            Coll = GetComponent<BoxCollider2D>();
        }

        public virtual void Start() {
            CalculateRaySpacing();
            CollisionMask = GameConfig.Instance.WorldMask;
        }

        public void UpdateRaycastOrigins() {
            var bounds = Coll.bounds;
            bounds.Expand(SkinWidth * -2);

            Origins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            Origins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
            Origins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
            Origins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        public void CalculateRaySpacing() {
            var bounds = Coll.bounds;
            bounds.Expand(SkinWidth * -2);

            var boundsWidth = bounds.size.x;
            var boundsHeight = bounds.size.y;


            HorizontalRayCount = Mathf.RoundToInt(boundsHeight / DstBetweenRays);
            VerticalRayCount = Mathf.RoundToInt(boundsWidth / DstBetweenRays);

            HorizontalRaySpacing = bounds.size.y / (HorizontalRayCount - 1);
            VerticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
        }

        public struct RaycastOrigins {
            public Vector2 TopLeft, TopRight;
            public Vector2 BottomLeft, BottomRight;
        }
    }
}