using Datenshi.Scripts.Controller;
using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class PhysicsUtil {
        private static int Sign(float y) {
            return y > 0 ? 1 : -1;
        }

        public static void DoVerticalCollisions(
            ref Vector2 vel,
            MovableEntity entity,
            ref CollisionStatus collStatus,
            Bounds2D bounds,
            Bounds2D skinBounds,
            LayerMask layerMask) {
            var motor = entity.Motor;
            var skinWidth = entity.SkinWidth;
            //Direction
            var direction = Sign(vel.y);

            var directionVector = new Vector2(0, vel.y * Time.deltaTime);
            // Bounds2D
            var bMin = bounds.Min;
            var bMax = bounds.Max;
            var positiveDir = direction == 1;

            var originX = skinBounds.Min.x;
            var originY = positiveDir ? bMax.y : bMin.y;
            var origin = new Vector2(originX, originY);
            var verticalRays = motor.VerticalRays;
            var width = skinBounds.Size.x;
            var spacing = width / (verticalRays - 1);
            var rayLength = directionVector.magnitude;
            var hitDown = false;
            for (var x = 0; x < verticalRays; x++) {
                var raycast = Physics2D.Raycast(origin, directionVector, rayLength, layerMask);
                Debug.DrawRay(origin, directionVector, raycast ? Color.green : Color.red);
                if (raycast && !raycast.collider.isTrigger) {
                    vel.y = raycast.distance / Time.deltaTime * direction;
                    rayLength = raycast.distance;
                    if (!hitDown) {
                        hitDown = direction == -1;
                    }

                    collStatus.Down = direction == -1;
                    collStatus.Up = direction == 1;
                }

                origin.x += spacing;
            }

            collStatus.Down = hitDown;
        }

        public static void DoHorizontalCollisions(
            ref Vector2 vel,
            MovableEntity entity,
            ref CollisionStatus collStatus,
            Bounds2D bounds,
            Bounds2D skinBounds,
            LayerMask layerMask) {
            var motor = entity.Motor;

            var direction = (int) Mathf.Sign(vel.x);

            var bMin = bounds.Min;
            var bMax = bounds.Max;

            var directionVector = new Vector2(vel.x * Time.deltaTime, 0);
            var positiveDir = direction == 1;
            var originX = positiveDir ? bMax.x : bMin.x;
            var originY = skinBounds.Min.y;
            var origin = new Vector2(originX, originY);
            var horizontalRays = motor.HorizontalRays;
            var height = skinBounds.Size.y;
            var spacing = height / (horizontalRays - 1);

            var rayLength = directionVector.magnitude;
            for (var y = 0; y < horizontalRays; y++) {
                var raycast = Physics2D.Raycast(origin, directionVector, rayLength, layerMask);
                Debug.DrawRay(origin, directionVector, raycast ? Color.green : Color.red);
                if (raycast) {
                    if (direction == 1) {
                        collStatus.Right = true;
                    } else {
                        collStatus.Left = true;
                    }

                    var slopeAngle = Vector2.Angle(raycast.normal, Vector2.up);
                    if (slopeAngle > motor.MaxAngle) {
                        //Hit wall
                        vel.x = raycast.distance / Time.deltaTime * direction;
                        rayLength = raycast.distance;

                        /*  if (Collisions.ClimbingSlope) {
                              moveAmount.y = Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                          }*/

                        collStatus.Left = direction == -1;
                        collStatus.Right = direction == 1;
                        continue;
                    }

                    //Slope was hit
                    //TODO: Slope handling
                }


                origin.y += spacing;
            }
        }

        public static void DoPhysics(MovableEntity entity, ref Vector2 vel, ref CollisionStatus collStatus) {
            var bounds = (Bounds2D) entity.Hitbox.bounds;
            //bounds.Center += vel * Time.deltaTime;
            DebugUtil.DrawBounds2D(bounds, Color.red);
            var skinBounds = bounds;
            skinBounds.Expand(-2 * entity.SkinWidth);
            var layerMask = GameResources.Instance.WorldMask;
            PhysicsUtil.DoVerticalCollisions(ref vel, entity, ref collStatus, bounds, skinBounds, layerMask);
            PhysicsUtil.DoHorizontalCollisions(ref vel, entity, ref collStatus, bounds, skinBounds, layerMask);
        }
    }
}