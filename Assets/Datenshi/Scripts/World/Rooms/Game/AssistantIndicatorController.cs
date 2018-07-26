using Datenshi.Scripts.Util.Singleton;
using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Game {
    public class AssistantIndicatorController : Singleton<AssistantIndicatorController> {
        public Animator Animator;
        public string ShowingKey = "Showing";

        public bool Showing {
            get;
            private set;
        }

        private void Update() {
            Animator.SetBool(ShowingKey, Showing);
        }

        public void Hide() {
            Showing = false;
        }

        public void Show(Vector2 direction) {
            Showing = true;
            var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}