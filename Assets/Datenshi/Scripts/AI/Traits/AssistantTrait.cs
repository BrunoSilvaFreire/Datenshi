using Datenshi.Scripts.AI.Behaviour;
using UnityEngine;
using UnityEngine.Networking;

namespace Datenshi.Scripts.AI.Traits {
    //TODO Fix
/*    public class AssistantTrait : Trait {
        public INavigable Assistant;
        public float MaxDistance = 10;
        private void Start() {
            Assistant.SetVariable(FollowingState NavigableTarget, PlayerController.Instance.CurrentINavigable);
            PlayerController.Instance.OnINavigableChanged.AddListener(OnChanged);
        }

        private void OnChanged(INavigable arg0, INavigable arg1) {
            Assistant.SetVariable(FollowingState.FollowTarget, arg1);
        }

        public override void Execute() {
            var e = PlayerController.Instance.CurrentINavigable;
            if (e == null) {
                return;
            }

            var ePos = e.Center;
            if (Vector2.Distance(ePos, Assistant.Center) > MaxDistance) {
                Assistant.transform.position = ePos;
            }
        }
    }*/
}