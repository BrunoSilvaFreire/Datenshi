using System;
using System.Collections.Generic;
using Datenshi.Scripts.AI.Pathfinding.Links;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding {
    [Serializable]
    public sealed class Node : IEquatable<Node> {
        public static readonly Node Invalid = new Node(Vector2Int.zero, NodeType.Invalid);

        [SerializeField, ReadOnly]
        private Vector2Int position;

        [SerializeField]
        private NodeType type;

        [SerializeField]
        private List<LinearLink> linearLinks = new List<LinearLink>();

        [SerializeField]
        private List<GravityLink> gravityLinks = new List<GravityLink>();

        public IEnumerable<Link> Links {
            get {
                foreach (var linearLink in linearLinks) {
                    yield return linearLink;
                }

                foreach (var gravityLink in gravityLinks) {
                    yield return gravityLink;
                }
            }
        }

        public Node() { }

        public Node(Vector2Int position, NodeType type) {
            this.position = position;
            this.type = type;
        }

        public NodeType Type {
            get {
                return type;
            }
        }

        public Vector2Int Position {
            get {
                return position;
            }
        }

        public bool IsInvalid {
            get {
                return Type == NodeType.Invalid;
            }
        }

        public bool IsValid {
            get {
                return !IsInvalid;
            }
        }

        public bool IsWalkable {
            get {
                return Type != NodeType.Invalid && Type != NodeType.Blocked && Type != NodeType.Empty;
            }
        }

        public bool IsEmpty {
            get {
                return Type != NodeType.Blocked;
            }
        }

        public bool IsBlocked {
            get {
                return Type == NodeType.Blocked;
            }
        }

        public int TotalLinearLinks {
            get {
                return linearLinks.Count;
            }
        }

        public int TotalGravityLinks {
            get {
                return gravityLinks.Count;
            }
        }

        public void AddLink(Link link) {
            var linearLink = link as LinearLink;
            if (linearLink != null) {
                linearLinks.Add(linearLink);
            }

            var gravityLink = link as GravityLink;
            if (gravityLink != null) {
                gravityLinks.Add(gravityLink);
            }
        }

        public override string ToString() {
            return string.Format("Node(Position: {0}, Type: {1})", position, type);
        }


        public bool Equals(Node other) {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || position.Equals(other.position);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var node = obj as Node;
            return node != null && Equals(node);
        }

        public override int GetHashCode() {
            return position.GetHashCode();
        }
    }

    public enum NodeType : byte {
        Invalid,
        Empty,
        Blocked,
        Platform,
        LeftEdge,
        RightEdge,
        LeftSlope,
        RightSlope,
        Solo
    }
}