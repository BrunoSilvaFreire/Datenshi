using System.Collections.Generic;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util;
using Lunari.Tsuki;
using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Game {
    public class AssistantIndicatorRoomController : AbstractRoomMember {
        public bool InitializeOnStart = true;
        public bool AllowTrigger = true;
        public Vector2 Direction;
        public bool DestroyPostLeft = true;

        private void Start() {
            if (Room == null) {
                var r = GetComponentInParent<Room>();
                if (r != null) {
                    r.AddMember(this);
                }
            }

            if (Room != null && InitializeOnStart) {
                Initialize();
            }
        }

        private void Initialize() {
            Room.OnMemberAdded.AddListener(OnMemberAdded);
            Room.OnMemberRemoved.AddListener(OnMemberRemoved);
            foreach (var member in Room.Members) {
                var l = member as LivingEntity;
                if (l == null) {
                    continue;
                }

                Register(l);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var i = AssistantIndicatorController.Instance;
            if (!i.Showing) {
                return;
            }

            var e = other.GetComponentInParent<Entity>();
            if (e == null || PlayerController.Instance.CurrentEntity != e) {
                return;
            }

            i.Hide();
            if (DestroyPostLeft) {
                Destroy(gameObject);
            }
        }

        private void OnMemberRemoved(IRoomMember arg0) {
            var l = arg0 as LivingEntity;
            if (l == null) {
                return;
            }

            pendentEntities.Remove(l);
        }

        private void OnMemberAdded(IRoomMember member) {
            var l = member as LivingEntity;
            if (l == null) {
                return;
            }

            Register(l);
        }

        private readonly List<LivingEntity> pendentEntities = new List<LivingEntity>();

        public void Check() {
            if (!AllowTrigger || !pendentEntities.IsEmpty()) {
                return;
            }

            Trigger();
        }

        private void Trigger() {
            AssistantIndicatorController.Instance.Show(Direction);
        }

        private void Register(LivingEntity livingEntity) {
            if (pendentEntities.Contains(livingEntity)) {
                return;
            }

            livingEntity.OnKilled.AddListener(() => {
                pendentEntities.Remove(livingEntity);
                Check();
            });
        }
    }
}