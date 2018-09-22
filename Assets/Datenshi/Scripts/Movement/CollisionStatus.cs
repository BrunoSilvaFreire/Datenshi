using System;
using Datenshi.Scripts.Util;
using UnityEngine;
using UnityUtilities.Misc;

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
    }

}