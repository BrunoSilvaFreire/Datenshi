using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class VelocityPitcher : MonoBehaviour {
        public MovableEntity MovableEntity;
        public AnimationCurve Pitch = AnimationCurve.Linear(0, 0.9F, 1, 1.1F);
        public AudioSource Source;

        private void Update() {
            Source.pitch = Pitch.Evaluate(MovableEntity.Velocity.magnitude / MovableEntity.MovementConfig.MaxSpeed);
        }
    }
}