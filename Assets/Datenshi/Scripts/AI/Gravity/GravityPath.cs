using Datenshi.Scripts.AI.Pathfinding;
using JetBrains.Annotations;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Gravity {
    public sealed class GravityPath {
        public GravityPath(Vector2 initialPosition, Vector2 initialVelocity) {
            InitialPosition = initialPosition;
            InitialVelocity = initialVelocity;
        }

        public Vector2 InitialPosition {
            get;
            private set;
        }

        public Vector2 InitialVelocity {
            get;
            private set;
        }

        [CanBeNull]
        public Node FinalNode {
            get {
                return finalNode;
            }
        }

        private Vector2[] path;
        private Node finalNode;

        public GravityPath(Vector2 initialPosition, Vector2 initialVelocity, Navmesh navmesh, float precision) : this(
            initialPosition, initialVelocity) {
            Calculate(navmesh, precision);
        }

        public Vector2[] GetPath(Navmesh navmesh, float precision) {
            if (path == null) {
                Calculate(navmesh, precision);
            }

            return path;
        }

        public void Calculate(Navmesh navmesh, float precision) {
            path = GravityUtil.CalculatePath(InitialPosition, InitialVelocity, navmesh, out finalNode, precision)
                .ToArray();
        }
    }
}