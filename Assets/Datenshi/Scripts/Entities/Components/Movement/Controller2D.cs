using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Components.Movement {
    public class Controller2D : RaycastController {
        public float FallingThroughPlatformResetTimer = 0.1f;
        public float MaxClimbAngle = 80f;
        public float MaxDescendAngle = 80f;

        [ShowInInspector]
        public CollisionInfo Collisions;

        [HideInInspector]
        public Vector2 PlayerInput;


        public override void Start() {
            base.Start();
            Collisions.FaceDir = 1;
        }

        public void Move(ref Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false) {
            UpdateRaycastOrigins();
            Collisions.Reset();
            Collisions.MoveAmountOld = moveAmount;
            PlayerInput = input;
            if (moveAmount.x != 0) {
                Collisions.FaceDir = (int) Mathf.Sign(moveAmount.x);
            }
            if (moveAmount.y < 0) {
                DescendSlope(ref moveAmount);
            }
            HorizontalCollisions(ref moveAmount);
            if (moveAmount.y != 0) {
                VerticalCollisions(ref moveAmount);
            }
            if (standingOnPlatform) {
                Collisions.Below = true;
            }
        }

        private void HorizontalCollisions(ref Vector2 moveAmount) {
            float directionX = Collisions.FaceDir;
            var rayLength = Mathf.Abs(moveAmount.x) + SkinWidth;

            if (Mathf.Abs(moveAmount.x) < SkinWidth) {
                rayLength = 2 * SkinWidth;
            }

            for (var i = 0; i < HorizontalRayCount; i++) {
                var rayOrigin = directionX == -1 ? Origins.BottomLeft : Origins.BottomRight;
                rayOrigin += Vector2.up * (HorizontalRaySpacing * i);
                var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

                if (!hit || hit.collider.isTrigger) {
                    continue;
                }
                if (hit.distance == 0) {
                    continue;
                }

                var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= MaxClimbAngle) {
                    if (Collisions.DescendingSlope) {
                        Collisions.DescendingSlope = false;
                        moveAmount = Collisions.MoveAmountOld;
                    }
                    var distanceToSlopeStart = 0f;
                    if (slopeAngle != Collisions.SlopeAngleOld) {
                        distanceToSlopeStart = hit.distance - SkinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmount, slopeAngle);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (!Collisions.ClimbingSlope || slopeAngle > MaxClimbAngle) {
                    moveAmount.x = (hit.distance - SkinWidth) * directionX;
                    rayLength = hit.distance;

                    if (Collisions.ClimbingSlope) {
                        moveAmount.y = Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    Collisions.Left = directionX == -1;
                    Collisions.Right = directionX == 1;
                }
            }
        }

        private void ClimbSlope(ref Vector2 moveAmount, float slopeAngle) {
            var moveDistance = Mathf.Abs(moveAmount.x);
            var climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

            if (moveAmount.y <= climbmoveAmountY) {
                moveAmount.y = climbmoveAmountY;
                moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                Collisions.Below = true;
                Collisions.ClimbingSlope = true;
                Collisions.SlopeAngle = slopeAngle;
            }
        }

        private void DescendSlope(ref Vector2 moveAmount) {
            var directionX = Mathf.Sign(moveAmount.x);
            var rayOrigin = directionX == -1 ? Origins.BottomRight : Origins.BottomLeft;
            var hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, CollisionMask);

            if (hit && !hit.collider.isTrigger) {
                var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0 && slopeAngle <= MaxDescendAngle) {
                    if (Mathf.Sign(hit.normal.x) == directionX) {
                        if (hit.distance - SkinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x)) {
                            var moveDistance = Mathf.Abs(moveAmount.x);
                            var descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                            moveAmount.y -= descendmoveAmountY;

                            Collisions.SlopeAngle = slopeAngle;
                            Collisions.DescendingSlope = true;
                            Collisions.Below = true;
                        }
                    }
                }
            }
        }

        private void VerticalCollisions(ref Vector2 moveAmount) {
            var directionY = Math.Sign(moveAmount.y);
            var rayLength = Mathf.Abs(moveAmount.y) + SkinWidth;

            for (var i = 0; i < VerticalRayCount; i++) {
                var rayOrigin = directionY == -1 ? Origins.BottomLeft : Origins.TopLeft;
                rayOrigin += Vector2.right * (VerticalRaySpacing * i + moveAmount.x);
                var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY, hit ? Color.green : Color.white);
                if (!hit || hit.collider.isTrigger) {
                    continue;
                }
                if (hit.collider.tag == "Through") {
                    if (directionY == 1 || hit.distance == 0) {
                        continue;
                    }
                    if (Collisions.FallingThroughPlatform) {
                        continue;
                    }
                    if (PlayerInput.y == -1) {
                        Collisions.FallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", FallingThroughPlatformResetTimer);
                        continue;
                    }
                }
                moveAmount.y = (hit.distance - SkinWidth) * directionY;
                rayLength = hit.distance;

                if (Collisions.ClimbingSlope) {
                    moveAmount.x = moveAmount.y / Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) *
                                   Mathf.Sign(moveAmount.x);
                }

                Collisions.Below = directionY == -1;
                Collisions.Above = directionY == 1;
            }

            if (Collisions.ClimbingSlope) {
                var directionX = Mathf.Sign(moveAmount.x);
                rayLength = Mathf.Abs(moveAmount.x) + SkinWidth;
                var rayOrigin = (directionX == -1 ? Origins.BottomLeft : Origins.BottomRight) +
                                Vector2.up * moveAmount.y;
                var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

                if (hit && !hit.collider.isTrigger) {
                    var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if (slopeAngle != Collisions.SlopeAngle) {
                        moveAmount.x = hit.distance * SkinWidth * directionX;
                        Collisions.SlopeAngle = slopeAngle;
                    }
                }
            }
        }

        private void ResetFallingThroughPlatform() {
            Collisions.FallingThroughPlatform = false;
        }

        public struct CollisionInfo {
            public bool Above, Below;
            public bool Left, Right;

            public bool ClimbingSlope;
            public bool DescendingSlope;
            public float SlopeAngle, SlopeAngleOld;
            public Vector2 MoveAmountOld;
            public int FaceDir;
            public bool FallingThroughPlatform;

            public void Reset() {
                Above = Below = false;
                Left = Right = false;
                ClimbingSlope = false;
                DescendingSlope = false;

                SlopeAngleOld = SlopeAngle;
                SlopeAngle = 0f;
            }
        }
    }
}