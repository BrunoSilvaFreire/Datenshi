using System.Collections;
using System.Collections.Generic;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.World.Rooms;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Misc {
    public class RequirementDoor : MonoBehaviour, IRoomMember {
        public SpriteRenderer[] Renderers;
        public Color CompletedColor;
        public AudioSource Source;
        public AudioClip Clip;
        public Collider2D Collider;

        [ShowInInspector, ReadOnly]
        private List<LivingEntity> deadRequired;

        [SerializeField]
        private UnityEvent onDestroyed;


        private void Start() {
            deadRequired = new List<LivingEntity>();
            foreach (var member in Room.Members) {
                var entity = member as LivingEntity;
                if (entity == null) {
                    continue;
                }

                deadRequired.Add(entity);
                entity.OnKilled.AddListener(() => {
                    deadRequired.Remove(entity);
                    Check();
                });
            }
        }

        private void Check() {
            if (!deadRequired.IsEmpty()) {
                return;
            }

            StartCoroutine(Open());
        }

        private IEnumerator Open() {
            yield return new WaitForSeconds(1);
            foreach (var renderer in Renderers) {
                renderer.color = CompletedColor;
            }

            Source.PlayOneShot(Clip);
            yield return new WaitForSeconds(1);
            foreach (var renderer in Renderers) {
                Destroy(renderer.gameObject);
            }

            Destroy(Collider.gameObject);
        }

        public UnityEvent OnDestroyed => onDestroyed;

        public Room Room {
            get;
            private set;
        }

        public bool RequestRoomMembership(Room room) {
            if (Room) {
                return false;
            }

            Room = room;
            return true;
        }
    }
}