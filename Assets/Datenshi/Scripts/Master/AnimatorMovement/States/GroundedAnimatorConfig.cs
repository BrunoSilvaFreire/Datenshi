using UnityEngine;

namespace Datenshi.Scripts.Master.AnimatorMovement.States {
    public class GroundedAnimatorConfig : RigidEntityConfig {
        public float JumpForce;
        public float RaycastLength = 0.1F;
        public AnimationCurve Acceleration;

        [SerializeField]
        public bool DashEllegible = true;
    }
}