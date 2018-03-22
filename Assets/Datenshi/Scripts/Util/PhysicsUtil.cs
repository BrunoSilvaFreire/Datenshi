using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Motors;
using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class PhysicsUtil {
        private static int Sign(float y) {
            return y > 0 ? 1 : -1;
        }

        public delegate void PhysicsHandler(
            Vector2 origin,
            Vector2 direction,
            LayerMask mask,
            RaycastHit2D hit,
            MovableEntity entity,
            ref CollisionStatus collStatus,
            ref Vector2 vel,
            Bounds2D bounds,
            Bounds2D skinBounds,
            LayerMask layerMask);

        public static RaycastHit2D? RaycastEntityVertical(
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
            RaycastHit2D? hit = null;

            for (var x = 0; x < verticalRays; x++) {
                var raycast = Physics2D.Raycast(origin, directionVector, rayLength, layerMask);
                Debug.DrawRay(origin, directionVector, raycast ? Color.cyan : Color.magenta);
                if (raycast && !raycast.collider.isTrigger) {
                    hit = raycast;
                    vel.y = raycast.distance / Time.deltaTime * direction;
                    rayLength = raycast.distance;
                    collStatus.Down = direction == -1;
                    collStatus.Up = direction == 1;
                }

                origin.x += spacing;
            }
            return hit;
        }

        public static void DoVerticalCollisions(
            ref Vector2 vel,
            MovableEntity entity,
            ref CollisionStatus collStatus,
            Bounds2D bounds,
            Bounds2D skinBounds,
            LayerMask layerMask,
            out RaycastHit2D? hit,
            PhysicsHandler handler) {
            var motor = entity.Motor;
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
            hit = null;
            var hitDown = false;
            for (var x = 0; x < verticalRays; x++) {
                var raycast = Physics2D.Raycast(origin, directionVector, rayLength, layerMask);
                Debug.DrawRay(origin, directionVector, raycast ? Color.green : Color.red);
                if (raycast && !raycast.collider.isTrigger) {
                    vel.y = raycast.distance / Time.deltaTime * direction;
                    rayLength = raycast.distance;
                    collStatus.Down = direction == -1;
                    collStatus.Up = direction == 1;
                    if (!hitDown) {
                        hitDown = direction == -1;
                    }
                    if (handler != null) {
                        handler(
                            origin,
                            directionVector,
                            layerMask,
                            raycast,
                            entity,
                            ref collStatus,
                            ref vel,
                            bounds,
                            skinBounds,
                            layerMask);
                    }
                }

                origin.x += spacing;
            }
            if (!hitDown) {
                collStatus.Down = false;
            }
        }

        public static RaycastHit2D? RaycastEntityHorizontal(
            Vector2 vel,
            MovableEntity entity
        ) {
            return RaycastEntityHorizontal(vel, entity, GameResources.Instance.WorldMask);
        }

        public static RaycastHit2D? RaycastEntityHorizontal(
            Vector2 vel,
            MovableEntity entity,
            LayerMask layerMask
        ) {
            var bounds = (Bounds2D) entity.Hitbox.bounds;
            //bounds.Center += vel * Time.deltaTime;
            DebugUtil.DrawBounds2D(bounds, Color.red);
            var skinBounds = bounds;
            skinBounds.Expand(-2 * entity.SkinWidth);
            return RaycastEntityHorizontal(vel, entity.Motor, bounds, skinBounds, layerMask);
        }

        public static RaycastHit2D? RaycastEntityHorizontal(
            Vector2 vel,
            Motor motor,
            Bounds2D bounds,
            Bounds2D skinBounds,
            LayerMask layerMask
        ) {
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
                if (raycast && !raycast.collider.isTrigger) {
                    return raycast;
                }


                origin.y += spacing;
            }
            return null;
        }

        public static void DoHorizontalCollisions(
            ref Vector2 vel,
            MovableEntity entity,
            ref CollisionStatus collStatus,
            Bounds2D bounds,
            Bounds2D skinBounds,
            LayerMask layerMask,
            out RaycastHit2D? hit,
            PhysicsHandler handler) {
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
            hit = null;
            for (var y = 0; y < horizontalRays; y++) {
                var raycast = Physics2D.Raycast(origin, directionVector, rayLength, layerMask);
                Debug.DrawRay(origin, directionVector, raycast ? Color.green : Color.red);
                if (raycast && !raycast.collider.isTrigger) {
                    hit = raycast;
                    collStatus.Left = direction == -1;
                    collStatus.Right = direction == 1;
                    var slopeAngle = Vector2.Angle(raycast.normal, Vector2.up);
                    if (slopeAngle > motor.MaxAngle) {
                        //Hit wall
                        vel.x = raycast.distance / Time.deltaTime * direction;
                        rayLength = raycast.distance;

                        /*  if (Collisions.ClimbingSlope) {
                              moveAmount.y = Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                          }*/

       
                        continue;
                    }

                    if (handler != null) {
                        handler(
                            origin,
                            directionVector,
                            layerMask,
                            raycast,
                            entity,
                            ref collStatus,
                            ref vel,
                            bounds,
                            skinBounds,
                            layerMask);
                    }
                }


                origin.y += spacing;
            }

            if (hit == null) {
                collStatus.Left = false;
                collStatus.Right = false;
            }
        }

        public static void DoPhysics(
            MovableEntity entity,
            ref Vector2 vel,
            ref CollisionStatus collStatus,
            out RaycastHit2D? verticalHit
        ) {
            DoPhysics(entity, ref vel, ref collStatus, out verticalHit, null);
        }

        public static void DoPhysics(
            MovableEntity entity,
            ref Vector2 vel,
            ref CollisionStatus collStatus,
            out RaycastHit2D? verticalHit,
            PhysicsHandler handler
        ) {
            DoPhysics(entity, ref vel, ref collStatus, out verticalHit, handler, handler);
        }

        public static void DoPhysics(
            MovableEntity entity,
            ref Vector2 vel,
            ref CollisionStatus collStatus,
            out RaycastHit2D? verticalHit,
            PhysicsHandler vertical,
            PhysicsHandler horizontal) {
            var bounds = (Bounds2D) entity.Hitbox.bounds;
            //bounds.Center += vel * Time.deltaTime;
            DebugUtil.DrawBounds2D(bounds, Color.red);
            var skinBounds = bounds;
            skinBounds.Expand(-2 * entity.SkinWidth);
            var layerMask = GameResources.Instance.WorldMask;
            DoVerticalCollisions(ref vel, entity, ref collStatus, bounds, skinBounds, layerMask, out verticalHit, vertical);
            DoHorizontalCollisions(ref vel, entity, ref collStatus, bounds, skinBounds, layerMask, out verticalHit, horizontal);
        }

        public static void DoPhysics(
            MovableEntity entity,
            ref Vector2 vel,
            ref CollisionStatus collStatus,
            out RaycastHit2D? verticalHit,
            out RaycastHit2D? horizontalHit,
            Bounds2D bounds,
            Bounds2D skinBounds,
            LayerMask layerMask) {
            DoVerticalCollisions(ref vel, entity, ref collStatus, bounds, skinBounds, layerMask, out verticalHit, null);
            DoHorizontalCollisions(ref vel, entity, ref collStatus, bounds, skinBounds, layerMask, out horizontalHit, null);
        }
    }
}