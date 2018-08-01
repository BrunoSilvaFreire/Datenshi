using Datenshi.Scripts.Util.Time;
using UnityEngine;

namespace Datenshi.Scripts.FX {
    public class SlowdownEffect : Effect {
        public AnimationCurve SlowdownCurve;
        public float SlowdownDuration = 1;

        public override void Execute(Vector3 location) {
            var c = TimeController.Instance;
            c.RequestAnimatedSlowdown(SlowdownCurve, SlowdownDuration);
        }
    }
}