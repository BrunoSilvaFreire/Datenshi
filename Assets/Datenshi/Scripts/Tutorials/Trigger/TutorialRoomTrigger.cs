using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.World;
using Datenshi.Tut;
using UnityEngine;

namespace Datenshi.Scripts.Tutorials.Trigger {
    public class TutorialRoomTrigger : AbstractRoomMember {
        public bool DestroyOnEnter;
        public Tutorial Tutorial;

        private void Start() {
            Room.OnObjectEnter.AddListener(OnEnter);
        }

        private void OnEnter(Collider2D coll) {
            var e = coll.GetComponentInParent<Entity>();
            if (e != PlayerController.Instance.CurrentEntity) {
                return;
            }

            Tutorial.StartTutorial();
            if (DestroyOnEnter) {
                Destroy(gameObject);
            }
        }
    }
}