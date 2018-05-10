using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.World.Rooms;
using Datenshi.Scripts.World.Rooms.Game;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Tutorial {
    public class TutorialTrigger : MonoBehaviour {
        public UITutorial TutorialPrefab;
        public bool HasCustomLocation;

        [ShowIf(nameof(HasCustomLocation))]
        public Transform CustomLocation;

        public void Show() {
            UITutorialBox.Instance.Show(this);
        }

        public void Hide() {
            UITutorialBox.Instance.Deregister(this);
        }
    }

    public class TutorialArea : TutorialTrigger, IRoomMember {
        public bool DestroyOnLeave;
        public bool DestroyAfterWaveEnd;

        [SerializeField]
        private UnityEvent onDestroyed;

        public UnityEvent OnDestroyed => onDestroyed;

        public Room Room {
            get;
            private set;
        }

        public bool RequestRoomMembership(Room room) {
            if (Room != null) {
                return false;
            }

            Room = room;
            return true;
        }

        private void Start() {
            if (DestroyAfterWaveEnd) {
                var spawner = Room.FindMember<Spawner>();
                if (spawner == null) {
                    return;
                }

                spawner.OnWaveCompleted.AddListener(OnCompleted);
            }
        }

        private void OnCompleted() {
            Delete();
        }

        private void Delete() {
            onDestroyed.Invoke();
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            var controller = PlayerController.Instance;
            if (other.isTrigger || other.GetComponentInParent<Entity>() != controller.CurrentEntity) {
                return;
            }

            Show();
        }

        private void OnTriggerExit2D(Collider2D other) {
            var controller = PlayerController.Instance;
            if (other.isTrigger || other.GetComponentInParent<Entity>() != controller.CurrentEntity) {
                return;
            }

            Hide();
            if (!DestroyOnLeave) {
                return;
            }

            Delete();
        }
    }
}