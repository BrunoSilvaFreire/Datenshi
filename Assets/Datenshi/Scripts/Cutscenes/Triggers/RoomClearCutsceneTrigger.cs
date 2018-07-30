using System.Collections.Generic;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.World.Rooms;
using Shiroi.Cutscenes.Triggers;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Cutscenes.Triggers {
    public class RoomClearCutsceneTrigger : CutsceneTrigger, IRoomMember {
        public bool AllowTrigger;
        public bool DestroyOnPlayerLeft;

        [SerializeField]
        private UnityEvent onDestroyed;

        [SerializeField]
        private UnityEvent onCompleted;


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
            if (Room == null) {
                var r = GetComponentInParent<Room>();
                if (r != null) {
                    r.AddMember(this);
                }
            }

            Initialize();
        }

        private void Initialize() {
            Room.OnMemberAdded.AddListener(OnMemberAdded);
            Room.OnMemberRemoved.AddListener(OnMemberRemoved);
            if (DestroyOnPlayerLeft) {
                Room.OnObjectExit.AddListener(OnLeft);
            }

            foreach (var member in Room.Members) {
                var l = member as LivingEntity;
                if (l == null) {
                    continue;
                }

                Register(l);
            }
        }

        private void OnLeft(Collider2D arg0) {
            var e = arg0.GetComponentInParent<Entity>();
            if (e != null && PlayerController.Instance.CurrentEntity == e) {
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

        private void Check() {
            if (!AllowTrigger || !pendentEntities.IsEmpty()) {
                return;
            }

            onCompleted.Invoke();
            Trigger();
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