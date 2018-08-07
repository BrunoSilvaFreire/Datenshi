using System;
using UnityEngine;

namespace Datenshi.Scripts.Math {
    [Serializable]
    public struct BezierCurve {
        public BezierPoint[] Points;
    }

    [Serializable]
    public struct BezierPoint {
        public Vector3 Position;
        public Vector3 StartTangent;
        public Vector3 EndTangent;
    }
}