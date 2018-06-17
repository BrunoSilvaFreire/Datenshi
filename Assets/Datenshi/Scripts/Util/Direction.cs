using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Util {
    [Serializable]
    public struct Direction : IComparable<Direction>, IComparable {
        public static readonly Direction Up = new Direction(DirectionValue.Zero, DirectionValue.Foward);
        public static readonly Direction Down = new Direction(DirectionValue.Zero, DirectionValue.Backward);
        public static readonly Direction Left = new Direction(DirectionValue.Backward, DirectionValue.Zero);
        public static readonly Direction Right = new Direction(DirectionValue.Foward, DirectionValue.Zero);

        public static readonly Direction UpLeft = new Direction(DirectionValue.Backward, DirectionValue.Foward);
        public static readonly Direction UpRight = new Direction(DirectionValue.Foward, DirectionValue.Foward);
        public static readonly Direction DownLeft = new Direction(DirectionValue.Backward, DirectionValue.Backward);
        public static readonly Direction DownRight = new Direction(DirectionValue.Foward, DirectionValue.Backward);

        public static readonly Direction Zero = new Direction(DirectionValue.Zero, DirectionValue.Zero);

        public static readonly Direction[] AllNonZero = {
            Up, Down, Left, Right,
            UpLeft, UpRight, DownLeft, DownRight
        };

        public static readonly Direction[] All = LoadAll();

        private static Direction[] LoadAll() {
            var a = new Direction[9];
            Array.Copy(AllNonZero, a, 8);
            a[0] = Zero;
            return a;
        }


        public int CompareTo(Direction other) {
            if (Y > other.Y) {
                return 1;
            }

            if (Y < other.Y) {
                return -1;
            }

            return ((int) X).CompareTo(other.X);
        }

        public int CompareTo(object obj) {
            if (ReferenceEquals(null, obj))
                return 1;
            if (!(obj is Direction))
                throw new ArgumentException("Object must be of type Direction");
            return CompareTo((Direction) obj);
        }

        public static bool operator ==(Direction left, Direction right) {
            return left.CompareTo(right) == 0;
        }

        public static bool operator !=(Direction left, Direction right) {
            return !(left == right);
        }

        public static bool operator <(Direction left, Direction right) {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Direction left, Direction right) {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Direction left, Direction right) {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Direction left, Direction right) {
            return left.CompareTo(right) >= 0;
        }

        public static Direction operator -(Direction d) {
            return new Direction(-d.X, -d.Y);
        }
        public bool Equals(Direction other) {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Direction && Equals((Direction) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        [Serializable]
        public struct DirectionValue {
            public static readonly DirectionValue Foward = new DirectionValue(1);
            public static readonly DirectionValue Zero = new DirectionValue(0);
            public static readonly DirectionValue Backward = new DirectionValue(-1);

            [ShowInInspector]
            private readonly int value;

            private DirectionValue(int value) {
                this.value = value;
            }

            public static implicit operator DirectionValue(int i) {
                switch (i) {
                    case 1:
                        return Foward;
                    case 0:
                        return Zero;
                    case -1:
                        return Backward;
                    default:
                        throw new ArgumentOutOfRangeException(i.ToString());
                }
            }

            public static implicit operator int(DirectionValue val) {
                return val.value;
            }

            public static DirectionValue FromVector(float val) {
                return FromVector(val, 0);
            }

            public static DirectionValue FromVector(float val, float limit) {
                if (val > limit) {
                    return Foward;
                }

                return val < -limit ? Backward : Zero;
            }
        }

        public float Angle => Mathf.Acos(Mathf.Clamp(Vector2.Dot(Vector2.zero, this), -1f, 1f)) * 57.29578f;


        public override string ToString() {
            return string.Format("({0}, {1})", (int) X, (int) Y);
        }

        public DirectionValue X;
        public DirectionValue Y;

        public static Direction GetDirection(float x, float y, float xLimit, float yLimit) {
            return new Direction(DirectionValue.FromVector(x, xLimit), DirectionValue.FromVector(y, yLimit));
        }

        public static Direction FromVector(Vector2 vector2) {
            return GetDirection(vector2.x, vector2.y);
        }

        private static Direction GetDirection(float x, float y) {
            if (y > 0) {
                if (x > 0) {
                    return UpRight;
                }

                return x < 0 ? UpLeft : Up;
            }

            if (y < 0) {
                if (x > 0) {
                    return DownRight;
                }

                return x < 0 ? DownLeft : Down;
            }

            if (x > 0) {
                return Right;
            }

            return x < 0 ? Left : Zero;
        }

        public static Direction FromVector(Vector2 vector2, float xLimit, float yLimit) {
            return GetDirection(vector2.x, vector2.y, xLimit, yLimit);
        }

        public Direction(DirectionValue x, DirectionValue y) {
            X = x;
            Y = y;
        }

        public static implicit operator Vector2(Direction dir) {
            return new Vector2(dir.X, dir.Y).normalized;
        }

        public static implicit operator Vector3(Direction dir) {
            return (Vector2) dir;
        }

        public static implicit operator Vector2Int(Direction dir) {
            return new Vector2Int(dir.X, dir.Y);
        }

        public static implicit operator Vector3Int(Direction dir) {
            return new Vector3Int(dir.X, dir.Y, 0);
        }

        public bool IsDiagonal() {
            return X != DirectionValue.Zero && Y != DirectionValue.Zero;
        }

        public bool IsZero() {
            return X == DirectionValue.Zero && Y == DirectionValue.Zero;
        }

        public static Direction FromX(float x) {
            if (x > 0) {
                return Right;
            }

            return x < 0 ? Left : Zero;
        }

        public static Direction FromX(int x) {
            if (x > 0) {
                return Right;
            }

            return x < 0 ? Left : Zero;
        }
    }
}