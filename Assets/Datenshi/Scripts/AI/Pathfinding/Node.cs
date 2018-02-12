using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding {
    [Serializable]
    public struct Node {
        public static readonly Node Invalid = new Node(Vector2Int.zero, NodeType.Invalid);

        [SerializeField, ReadOnly]
        private Vector2Int position;

        [SerializeField]
        private NodeType type;


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
                return Type == NodeType.Empty;
            }
        }
    }

    public enum NodeType : byte {
        Invalid,
        Empty,
        Blocked,
        Platform,
        LeftEdge,
        RightEdge,
        Solo
    }
}