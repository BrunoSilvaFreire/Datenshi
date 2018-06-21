using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Datenshi.Scripts.AI.Pathfinding {
    public class Navmesh : Singleton<Navmesh> {
        public static readonly Vector2 BoxCastSize = new Vector2(0.9F, 0.9F);

        [SerializeField]
        private Node[] nodes;

        [SerializeField, HideInInspector]
        private Vector2Int min;

        [SerializeField, HideInInspector]
        private Vector2Int max;

        public Grid Grid;
        public LayerMask LayerMask;
        private Vector2 cachedBoxcastSize;
        private bool boxcastCached;

        public Vector2 BoxcastSize {
            get {
                if (boxcastCached) {
                    return cachedBoxcastSize;
                }

                cachedBoxcastSize = Grid.cellSize;
                cachedBoxcastSize.Scale(GameResources.Instance.NavmeshBoxcastDownsizeScale);
                boxcastCached = true;
                return cachedBoxcastSize;
            }
        }

        [ShowInInspector]
        public Vector2Int Min {
            get {
                return min;
            }
            set {
                AssignMinMax(value, max);
            }
        }

        [ShowInInspector]
        public Vector2Int Max {
            get {
                return max;
            }
            set {
                AssignMinMax(min, value);
            }
        }

        public int YSize => Max.y - Min.y + 1;

        public int XSize => Max.x - Min.x + 1;

        public Vector2 Center => (MinWorld + MaxWorld + Vector2.one) / 2;

        public Vector2 Size => new Vector2(MaxWorld.x - MinWorld.x + 1, MaxWorld.y - MinWorld.y + 1);

        public Vector2 MaxWorld => Grid.CellToWorld(Max.ToVector3());

        public Vector2 MinWorld => Grid.CellToWorld(Min.ToVector3());

        public int TotalSize => XSize * YSize;

        public Node[] Nodes => nodes;

        private void AssignMinMax(Vector2Int newMin, Vector2Int newMax) {
            min.x = Mathf.Min(newMin.x, newMax.x);
            min.y = Mathf.Min(newMin.y, newMax.y);
            max.x = Mathf.Max(newMin.x, newMax.x);
            max.y = Mathf.Max(newMin.y, newMax.y);
        }

        private IEnumerable<Vector2Int> GetAllPossiblePositions() {
            for (var y = Min.y; y <= Max.y; y++) {
                for (var x = Min.x; x <= Max.x; x++) {
                    yield return new Vector2Int(x, y);
                }
            }
        }

        public void Regenerate() {
            nodes = new Node[TotalSize];
            foreach (var pos in GetAllPossiblePositions()) {
                Nodes[GetNodeIndex(pos)] = CreateNode(pos);
            }
        }

        private Node CreateNode(Vector2Int pos) {
            return new Node(pos, CheckNodeType(pos, BoxCastSize));
        }

        public Node this[uint index] => index >= nodes.Length ? Node.Invalid : nodes[index];

        public Vector2 GetWorldPosition(uint id, int z = 0) {
            var node = this[id];
            return node.IsInvalid ? Vector2.zero : WorldPosCenter(node);
        }

        private bool BoxCast(int x, int y, Vector2 size) {
            return BoxCast(new Vector3Int(x, y, 0), size);
        }

        private Vector2 ToCenter(Vector3Int mapPosition) {
            return Grid.CellToWorld(mapPosition) + Grid.cellSize / 2;
        }

        private bool BoxCast(Vector3Int mapPosition, Vector2 size) {
            var b = Physics2D.BoxCastAll(ToCenter(mapPosition), size, 0f, Vector2.zero, 1F, LayerMask);
            if (b.All(hit => hit.collider.isTrigger)) {
                return false;
            }

            return !b.IsNullOrEmpty();
        }

        private NodeType CheckNodeType(Vector2Int pos, Vector2 size) {
            var mapPosition = pos.ToVector3();
            if (BoxCast(mapPosition, size)) {
                //Inside an object
                return NodeType.Blocked;
            }

            var x = mapPosition.x;
            var y = mapPosition.y;
            if (!BoxCast(x, y - 1, size)) {
                return NodeType.Empty;
            }

            //Is on solid ground, check for edges
            var leftLowBoxCast = BoxCast(x - 1, y - 1, size);
            var rightLowBoxCast = BoxCast(x + 1, y - 1, size);
            var leftBoxCast = BoxCast(x - 1, y, size);
            var rightBoxCast = BoxCast(x + 1, y, size);
            if (leftLowBoxCast && rightLowBoxCast && !leftBoxCast && !rightBoxCast) {
                return NodeType.Platform;
            }

            if (leftLowBoxCast || leftBoxCast) {
                return NodeType.RightEdge;
            }

            if (rightLowBoxCast || rightBoxCast) {
                return NodeType.LeftEdge;
            }

            return NodeType.Solo;
        }

        public bool IsOutOfBounds(Vector3 worldPosition) {
            return IsOutOfBounds(worldPosition.x, worldPosition.y);
        }

        public bool IsOutOfBounds(float x, float y) {
            return IsOutOfBounds(x, y, MinWorld, MaxWorld);
        }

        public bool IsOutOfGridBounds(Vector2Int position, Direction direction) {
            return IsOutOfGridBounds(position + direction);
        }

        public bool IsOutOfGridBounds(Vector2Int pos) {
            return IsOutOfGridBounds(pos.x, pos.y);
        }

        public bool IsOutOfGridBounds(int posX, int posY) {
            return posX > Max.x || posX < Min.x || posY > Max.y || posY < Min.y;
        }

        public bool IsOutOfGridBounds(int posX, int posY, Vector2Int min, Vector2Int max) {
            return posX > max.x || posX < min.x || posY > max.y || posY < min.y;
        }

        public bool IsOutOfBounds(float posX, float posY, Vector2 min, Vector2 max) {
            return posX > max.x || posX < min.x || posY > max.y || posY < min.y;
        }

        public int GetNodeIndex(Node node) {
            return GetNodeIndex(node.Position);
        }

        public int GetNodeIndex(Vector2Int pos) {
            if (IsOutOfGridBounds(pos)) {
                return -1;
            }

            var x = pos.x - Min.x;
            var y = pos.y - Min.y;
            return y * XSize + x;
        }

        public Node GetNeightboor(Node node, Direction direction) {
            return GetNode(node.Position + direction);
        }

        public Node GetNode(Vector2Int pos) {
            return GetNode(GetNodeIndex(pos));
        }

        public Node GetNode(int index) {
            return nodes[index];
        }


        public Node GetNodeAtWorld(Vector2 position) {
            if (IsOutOfBounds(position)) {
                return Node.Invalid;
            }

            var pos = Grid.WorldToCell(position).ToVector2();
            return GetNode(pos);
        }

        public Vector2 WorldPos(Vector2Int position) {
            return Grid.CellToWorld(position.ToVector3());
        }

        public Vector2 WorldPosCenter(uint position) {
            return WorldPosCenter(this[position]);
        }

        public Vector2 WorldPosCenter(Vector2Int position) {
            return WorldPos(position) + (Vector2) Grid.cellSize / 2;
        }

        public Vector2 WorldPosCenter(Node position) {
            return WorldPosCenter(position.Position);
        }

        public bool IsOnSamePlatform(Node node, int other) {
            return IsOnSamePlatform(node, GetNode(other));
        }

        private bool IsOnSamePlatform(Node a, Node b) {
            if (a == null || b == null) {
                return false;
            }

            var aPos = a.Position;
            var bPos = b.Position;
            if (aPos.y != bPos.y) {
                return false;
            }

            var current = a;
            var direction = GetXDirection(aPos, bPos);
            while (current != b) {
                current = GetNeightboor(current, direction);
                if (!current.IsWalkable) {
                    return false;
                }
            }

            return true;
        }

        private Direction GetXDirection(Vector2Int aPos, Vector2Int bPos) {
            if (aPos.x == bPos.x) {
                return Direction.Zero;
            }

            return aPos.x > bPos.x ? Direction.Left : Direction.Right;
        }
    }
}