using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Game {
    public static class GameUtility {
        public static bool ContainsPlayerEntity(this Collider2D collider2D) {
            var pe = PlayerController.Instance.CurrentEntity;
            var e = collider2D.GetComponentInParent<Entity>();
            return pe != null && pe == e;
        }
    }
}