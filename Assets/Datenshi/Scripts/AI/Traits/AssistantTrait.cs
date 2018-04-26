using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.AI.Traits {
    public class AssistantTrait : Trait {
        public MovableEntity Assistant;
        public float MaxDistance = 10;
        private void Start() {
            Assistant.SetVariable(FollowingState.EntityTarget, PlayerController.Instance.CurrentEntity);
            PlayerController.Instance.OnEntityChanged.AddListener(OnChanged);
        }

        private void OnChanged(Entity arg0, Entity arg1) {
            Assistant.SetVariable(FollowingState.EntityTarget, arg1);
        }

        public override void Execute() {
            var e = PlayerController.Instance.CurrentEntity;
            if (e == null) {
                return;
            }

            var ePos = e.Center;
            if (Vector2.Distance(ePos, Assistant.Center) > MaxDistance) {
                Assistant.transform.position = ePos;
            }
        }
    }
}