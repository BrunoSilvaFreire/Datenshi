using System;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Gravity {
    public static class GravityUtil {
#if UNITY_EDITOR
        public delegate void PathCalculatedGravityListener(Vector2 a, Vector2 b, RaycastHit2D hit);

        public static event PathCalculatedGravityListener OnCalculated;
#endif
        public static List<Vector2> CalculatePath(
            Vector2 pos,
            Vector2 initialSpeed,
            Vector2 boxcastSize,
            Navmesh tileMap,
            out Node finalNode
        ) {
            return CalculatePath(pos, initialSpeed, boxcastSize, tileMap, out finalNode,
                GameResources.Instance.DefaultPrecision);
        }

        public static List<Vector2> CalculatePath(
            Vector2 pos,
            Vector2 initialSpeed,
            Vector2 boxcastSize,
            Navmesh tileMap,
            out Node finalNode,
            float precision) {
            finalNode = null;
            float time = 0;
            if (precision > GameResources.Instance.MaxPrecision) {
                return CollectionUtil.EmptyList<Vector2>();
            }

            var timeIncrementation = 1 / precision;
            var mask = tileMap.LayerMask;
            var list = new List<Vector2>();
            while (finalNode == null) {
                var y = pos.y + initialSpeed.y * time + GameResources.Instance.Gravity * Mathf.Pow(time, 2) / 2;
                var x = pos.x + initialSpeed.x * time;
                if (tileMap.IsOutOfBounds(x, y)) {
                    break;
                }

                var newPos = new Vector2(x, y);
                if (list.Count > 0) {
                    var last = list.Last();
                    var dir = newPos - last;
                    var raycast = HitDetect(last, dir, mask, boxcastSize);
#if UNITY_EDITOR
                    NotifyOnCalculated(last, newPos, raycast);
#endif
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
#if UNITY_EDITOR

        private static void NotifyOnCalculated(Vector2 a, Vector2 b, RaycastHit2D hit) {
            var handler = OnCalculated;
            if (handler != null)
                handler(a, b, hit);
        }
#endif
    }
}