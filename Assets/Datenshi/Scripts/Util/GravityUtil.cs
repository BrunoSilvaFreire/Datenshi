using System;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.AI.Pathfinding;
using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class GravityUtil {
        public const float DefaultGravityForce = -9.81f;

        public static List<Vector2> CalculatePath(
            Vector2 pos,
            Vector2 initialSpeed,
            float timeIncrementation,
            float gravity,
            Navmesh tileMap,
            Vector2 boxcastSize,
            out Node finalNode) {
            finalNode = null;
            float time = 0;
            if (timeIncrementation < 0) {
                return Enumerable.Empty<Vector2>().ToList();
            }
            var mask = tileMap.LayerMask;
            var list = new List<Vector2>();
            while (finalNode == null) {
                var y = pos.y + initialSpeed.y * time + gravity * Mathf.Pow(time, 2) / 2;
                var x = pos.x + initialSpeed.x * time;
                if (tileMap.IsOutOfBounds(x, y)) {
                    break;
                }
                var newPos = new Vector2(x, y);
                if (list.Count > 0) {
                    var last = list.Last();
                    var dir = newPos - last;
                    var raycast = HitDetect(last, dir, mask, boxcastSize);
                    if (raycast) {
                        //GizmosUtil.ArrowDebug(last, dir, Color.red);
                        var hitPoint = raycast.point;
                        list.Add(hitPoint);
                        Node hitNode;
                        try {
                            hitNode = tileMap.GetNodeAtWorld(hitPoint);
                        } catch (Exception e) {
                            continue;
                        }
                        if (hitNode != null) {
                            finalNode = hitNode;
                        }
                        break;
                    }
                    //GizmosUtil.ArrowDebug(last, dir, Color.yellow);
                }
                list.Add(newPos);
                time += timeIncrementation;
            }

            return list;
        }

        private static RaycastHit2D HitDetect(
            Vector2 position,
            Vector2 dir,
            LayerMask tileMapWorldMask,
            Vector2 boxcastSize) {
            var pos = position;
            pos.y += boxcastSize.y / 2;
            return Physics2D.BoxCast(pos, boxcastSize, 0, dir, dir.magnitude, tileMapWorldMask);
        }

        public static float CalculateDistance(List<Vector2> path) {
            float distance = 0;
            for (var i = 1; i < path.Count - 1; i++) {
                var point = path[i];
                var previous = path[i - 1];
                distance += Vector2.Distance(point, previous);
            }
            return distance;
        }

        public static void DrawDebug(Vector2[] path, Color initColor) {
            var color = initColor;
            var maxAlpha = color.a;
            for (var i = 1; i < path.Length; i++) {
                var point = path[i];
                var previous = path[i - 1];

                var newAlpha = maxAlpha * i / path.Length;
                color.a = newAlpha;
                Debug.DrawLine(previous, point, color);
/*                if (i % 8 != 0) {
                    Debug.DrawLine(previous, point, color);
                } else {
                    GizmosUtil.ArrowDebug(previous, point - previous, color);
                }*/
            }
        }
    }
}