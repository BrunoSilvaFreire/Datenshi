using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Motor.States.Listeners;
using Datenshi.Scripts.Game;
using Shiroi.FX.Utilities;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Datenshi.Scripts.Master {
    public class DashFXHandler : MonoBehaviour {
        public float ChangeSpeed = 0.4F;
        public PostProcessVolume Volume;
        public Range AudioRange;

        private void Update() {
            var e = PlayerController.Instance.CurrentEntity as MovableEntity;
            bool dashing;
            dashing = false;
            if (e != null) {
                dashing = e.GetVariable(DashListener.DashingLastFrame);
            }

            var target = dashing ? 1 : 0;
            Volume.weight = Mathf.Lerp(Volume.weight, target, ChangeSpeed);
        }
    }
}