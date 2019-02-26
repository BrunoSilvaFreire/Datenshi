using System;
using Datenshi.Scripts.Util;
using Lunari.Tsuki.Misc;
using UnityEngine;

namespace Datenshi.Scripts.Movement {
    [Serializable]
    public struct CollisionStatus {
        [SerializeField]
        private bool up, down, left, right;

        public UnitValue HorizontalCollisionDir {
            get {
                if (left == right) {
                    return UnitValue.Zero;
                }

                if (left) {
                    return UnitValue.MinusOne;
                }

                if (right) {
                    return UnitValue.One;
                }

                throw new WTFException();
            }
        }

        public bool Up {
            get {
                return up;
            }
            set {
                up = value;
            }
        }

        public bool Down {
            get {
                return down;
            }
            set {
                down = value;
            }
        }

        public bool Left {
            get {
                return left;
            }
            set {
                left = value;
            }
        }

        public bool Right {
            get {
                return right;
            }
            set {
                right = value;
            }
        }

        public bool HasAny => down || up || left || right;
        public bool HasHorizontal => HorizontalCollisionDir != 0;

        public override string ToString() {
            return $"{nameof(Up)}: {Up}, {nameof(Down)}: {Down}, {nameof(Left)}: {Left}, {nameof(Right)}: {Right}";
        }
    }
}