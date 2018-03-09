using Datenshi.Scripts.Entities;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class HandlesUtil {
        public static void DrawBox2DWire(Vector2 center, Vector2 size) {
            var halfWidth = size.x / 2;
            var halfHeight = size.y / 2;
            var bottomLeft = new Vector2(center.x - halfWidth, center.y - halfHeight);
            var topLeft = new Vector2(center.x - halfWidth, center.y + halfHeight);
            var bottomRight = new Vector2(center.x + halfWidth, center.y - halfHeight);
            var topRight = new Vector2(center.x + halfWidth, center.y + halfHeight);
            Handles.DrawLine(bottomLeft, topLeft);
            Handles.DrawLine(topLeft, topRight);
            Handles.DrawLine(topRight, bottomRight);
            Handles.DrawLine(bottomRight, bottomLeft);
        }

        public static void DrawBox2DWire(Vector2 center, Vector2 size, Color color) {
            var halfWidth = size.x / 2;
            var halfHeight = size.y / 2;
            var bottomLeft = new Vector2(center.x - halfWidth, center.y - halfHeight);
            var topLeft = new Vector2(center.x - halfWidth, center.y + halfHeight);
            var bottomRight = new Vector2(center.x + halfWidth, center.y - halfHeight);
            var topRight = new Vector2(center.x + halfWidth, center.y + halfHeight);
            Handles.color = color;
            Handles.DrawLine(bottomLeft, topLeft);
            Handles.DrawLine(topLeft, topRight);
            Handles.DrawLine(topRight, bottomRight);
            Handles.DrawLine(bottomRight, bottomLeft);
        }

        public static void DrawBox2D(Vector2 center, Vector2 size) {
            var halfWidth = size.x / 2;
            var halfHeight = size.y / 2;
            var bottomLeft = new Vector2(center.x - halfWidth, center.y - halfHeight);
            var topLeft = new Vector2(center.x - halfWidth, center.y + halfHeight);
            var bottomRight = new Vector2(center.x + halfWidth, center.y - halfHeight);
            var topRight = new Vector2(center.x + halfWidth, center.y + halfHeight);
            Handles.DrawSolidRectangleWithOutline(new Vector3[] {bottomLeft, topLeft, topRight, bottomRight},
                Color.white, Color.black);
        }

        public static void DrawBox2D(Vector2 center, Vector2 size, Color color) {
            var halfWidth = size.x / 2;
            var halfHeight = size.y / 2;
            var bottomLeft = new Vector2(center.x - halfWidth, center.y - halfHeight);
            var topLeft = new Vector2(center.x - halfWidth, center.y + halfHeight);
            var bottomRight = new Vector2(center.x + halfWidth, center.y - halfHeight);
            var topRight = new Vector2(center.x + halfWidth, center.y + halfHeight);
            Handles.color = color;
            Handles.DrawSolidRectangleWithOutline(new Vector3[] {bottomLeft, topLeft, topRight, bottomRight},
                Color.white, Color.black);
        }

        public static void DrawRay(Vector2 pos, Vector2 dir) {
            Handles.DrawLine(pos, pos + dir);
        }

        public static void ArrowGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f) {
            Gizmos.DrawLine(pos, pos + direction);
            DrawArrowEnd(true, pos, direction, Handles.color, arrowHeadLength, arrowHeadAngle);
        }

        public static void ArrowGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f) {
            Gizmos.DrawLine(pos, pos + direction);
            DrawArrowEnd(true, pos, direction, color, arrowHeadLength, arrowHeadAngle);
        }

        public static void ArrowDebug(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f) {
            Debug.DrawRay(pos, direction);
            DrawArrowEnd(false, pos, direction, Handles.color, arrowHeadLength, arrowHeadAngle);
        }

        public static void ArrowDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f) {
            Debug.DrawRay(pos, direction, color);
            DrawArrowEnd(false, pos, direction, color, arrowHeadLength, arrowHeadAngle);
        }

        private static void DrawArrowEnd(bool gizmos, Vector3 pos, Vector3 direction, Color color,
            float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f) {
            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back;
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back;
            var up = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back;
            var down = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back;
            Handles.color = color;
            DrawRay(pos + direction, right * arrowHeadLength);
            DrawRay(pos + direction, left * arrowHeadLength);
            DrawRay(pos + direction, up * arrowHeadLength);
            DrawRay(pos + direction, down * arrowHeadLength);
        }
    }

    public static class DebugUtil {
        public static void DrawBox2DWire(Vector2 center, Vector2 size) {
            var halfWidth = size.x / 2;
            var halfHeight = size.y / 2;
            var bottomLeft = new Vector2(center.x - halfWidth, center.y - halfHeight);
            var topLeft = new Vector2(center.x - halfWidth, center.y + halfHeight);
            var bottomRight = new Vector2(center.x + halfWidth, center.y - halfHeight);
            var topRight = new Vector2(center.x + halfWidth, center.y + halfHeight);
            Debug.DrawLine(bottomLeft, topLeft);
            Debug.DrawLine(topLeft, topRight);
            Debug.DrawLine(topRight, bottomRight);
            Debug.DrawLine(bottomRight, bottomLeft);
        }

        public static void DrawBox2DWire(Vector2 center, Vector2 size, Color color) {
            var halfWidth = size.x / 2;
            var halfHeight = size.y / 2;
            var bottomLeft = new Vector2(center.x - halfWidth, center.y - halfHeight);
            var topLeft = new Vector2(center.x - halfWidth, center.y + halfHeight);
            var bottomRight = new Vector2(center.x + halfWidth, center.y - halfHeight);
            var topRight = new Vector2(center.x + halfWidth, center.y + halfHeight);
            Debug.DrawLine(bottomLeft, topLeft, color);
            Debug.DrawLine(topLeft, topRight, color);
            Debug.DrawLine(topRight, bottomRight, color);
            Debug.DrawLine(bottomRight, bottomLeft, color);
        }


        public static void DrawRay(Vector2 pos, Vector2 dir) {
            Debug.DrawLine(pos, pos + dir);
        }

        public static void DrawBounds2D(Bounds bounds, Color color) {
            DrawBox2DWire(bounds.center, bounds.size, color);
        }

        public static void DrawBounds2D(Bounds2D bounds, Color color) {
            DrawBox2DWire(bounds.Center, bounds.Size, color);
        }
    }

    public static class GizmosUtil {
        public static void DrawBox2DWire(Vector2 center, Vector2 size) {
            var halfWidth = size.x / 2;
            var halfHeight = size.y / 2;
            var bottomLeft = new Vector2(center.x - halfWidth, center.y - halfHeight);
            var topLeft = new Vector2(center.x - halfWidth, center.y + halfHeight);
            var bottomRight = new Vector2(center.x + halfWidth, center.y - halfHeight);
            var topRight = new Vector2(center.x + halfWidth, center.y + halfHeight);
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
        }

        public static void DrawBox2DWire(Vector2 center, Vector2 size, Color color) {
            var halfWidth = size.x / 2;
            var halfHeight = size.y / 2;
            var bottomLeft = new Vector2(center.x - halfWidth, center.y - halfHeight);
            var topLeft = new Vector2(center.x - halfWidth, center.y + halfHeight);
            var bottomRight = new Vector2(center.x + halfWidth, center.y - halfHeight);
            var topRight = new Vector2(center.x + halfWidth, center.y + halfHeight);
            Gizmos.color = color;
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
        }


        public static void DrawRay(Vector2 pos, Vector2 dir) {
            Gizmos.DrawLine(pos, pos + dir);
        }

        public static void ArrowGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f) {
            Gizmos.DrawLine(pos, pos + direction);
            DrawArrowEnd(true, pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle);
        }

        public static void ArrowGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f) {
            Gizmos.DrawLine(pos, pos + direction);
            DrawArrowEnd(true, pos, direction, color, arrowHeadLength, arrowHeadAngle);
        }

        public static void ArrowDebug(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f) {
            Debug.DrawRay(pos, direction);
            DrawArrowEnd(false, pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle);
        }

        public static void ArrowDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f) {
            Debug.DrawRay(pos, direction, color);
            DrawArrowEnd(false, pos, direction, color, arrowHeadLength, arrowHeadAngle);
        }

        private static void DrawArrowEnd(bool gizmos, Vector3 pos, Vector3 direction, Color color,
            float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f) {
            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back;
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back;
            var up = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back;
            var down = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back;
            Gizmos.color = color;
            DrawRay(pos + direction, right * arrowHeadLength);
            DrawRay(pos + direction, left * arrowHeadLength);
            DrawRay(pos + direction, up * arrowHeadLength);
            DrawRay(pos + direction, down * arrowHeadLength);
        }

        public static void DrawBounds2D(Bounds bounds, Color color) {
            DrawBox2DWire(bounds.center, bounds.size, color);
        }
    }
}