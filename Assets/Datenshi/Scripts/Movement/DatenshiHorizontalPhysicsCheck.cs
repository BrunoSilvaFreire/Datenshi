using Datenshi.Scripts.Movement.Config;
using UnityEngine;
using UPM;
using UPM.Motors;
using UPM.Motors.Config;
using UPM.Physics;
using UPM.Util;

namespace Datenshi.Scripts.Movement {
    public class DatenshiHorizontalPhysicsCheck : SimplePhysicsCheck {
        public DatenshiHorizontalPhysicsCheck(CustomVelocityProvider customVelocityProvider = null) : base(
            customVelocityProvider) { }

        public RaycastHit2D? LastHit {
            get;
            private set;
        }

        protected override void DoCheck(IMovable user, ref Vector2 vel, ref CollisionStatus collStatus, LayerMask mask,
            Bounds2D bounds, Bounds2D shrinkedBounds) {
            var direction = (int) Mathf.Sign(vel.x);

            var bMin = bounds.Min;
            var bMax = bounds.Max;

            var horizontalRays = user.HorizontalRaycasts;
            var directionVector = new Vector2(vel.x * Time.deltaTime, 0);
            var positiveDir = direction == 1;
            var originX = positiveDir ? bMax.x : bMin.x;
            var originY = shrinkedBounds.Min.y;
            var origin = new Vector2(originX, originY);
            var height = shrinkedBounds.Size.y;
            var spacing = height / (horizontalRays - 1);

            var rayLength = directionVector.magnitude;
            LastHit = null;
            var config = user.GetMotorConfig<GroundMotorConfig>();
            var maxAngle = config == null ? 0 : config.MaxAngle;
            for (var y = 0; y < horizontalRays; y++) {
                var raycast = Physics2D.Raycast(origin, directionVector, rayLength, mask);
                Debug.DrawRay(origin, directionVector, raycast ? Color.green : Color.red);
                if (raycast && !raycast.collider.isTrigger && raycast.distance < rayLength) {
                    LastHit = raycast;
                    collStatus.Left = direction == -1;
                    collStatus.Right = direction == 1;
                    var slopeAngle = Vector2.Angle(raycast.normal, Vector2.up);
                    if (config != null) {
                        if (slopeAngle > maxAngle) {
                            //Hit wall
                            vel.x = raycast.distance / Time.deltaTime * direction;
                            rayLength = raycast.distance;
                            /*  if (Collisions.ClimbingSlope) {
                                  moveAmount.y = Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                              }*/
                            continue;
                        }
                    } else {
                        //Hit wall
                        if (Mathf.RoundToInt(slopeAngle) % 90 == 0) {
                            vel.x = 0;
                        } else {
                            vel.x = raycast.distance / Time.deltaTime * direction;
                            rayLength = raycast.distance;
                            vel.y = Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(vel.x);
                            continue;
                        }
                    }
                }


                origin.y += spacing;
            }

            if (LastHit != null) {
                return;
            }

            collStatus.Left = false;
            collStatus.Right = false;
        }
    }
}