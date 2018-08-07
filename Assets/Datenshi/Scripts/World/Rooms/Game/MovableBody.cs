using Datenshi.Scripts.Math;
using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Game {
    public class MovableBody : MonoBehaviour {
        public BezierCurve Curve;
        public Transform MovableTransform;
        public bool Loop;
    }
}