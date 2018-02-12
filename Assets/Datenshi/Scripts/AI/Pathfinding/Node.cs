﻿using System;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.AI.Pathfinding.Links;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding {
    [Serializable]
    public sealed class Node {
        public static readonly Node Invalid = new Node(Vector2Int.zero, NodeType.Invalid);

        [SerializeField, ReadOnly]
        private Vector2Int position;

        [SerializeField]
        private NodeType type;

        [SerializeField]
        private List<LinearLink> linearLinks = new List<LinearLink>();

        public IEnumerable<Link> Links {
            get {
                foreach (var linearLink in linearLinks) {
                    yield return linearLink;
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
                return Type == NodeType.Empty;
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

        public void AddLink(Link link) {
            var item = link as LinearLink;
            if (item != null) {
                linearLinks.Add(item);
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