using System;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Entities.Motors;
using Datenshi.Scripts.Entities.Motors.State;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Interaction;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    /// <inheritdoc />
    /// <summary>
    /// Uma entidade que se move.
    /// <br />
    /// A maneira com que ela se move depende de seu <see cref="F:Datenshi.Scripts.Entities.MovableEntity.Motor" />.
    /// </summary>
    public class MovableEntity : LivingEntity {
        public const string MovementGroup = "Movement";

        [NonSerialized, ShowInInspector, ReadOnly, TitleGroup(MovementGroup)]
        public MotorStateMachine MovementStateMachine;

        /// <summary>
        /// O motor a ser utilizado por essa entidade para movimentação
        /// </summary>
        [TitleGroup(MovementGroup, "Informações sobre a maneira de locomoção desta entidade")]
        public Motor Motor;

        public AnimationCurve AccelerationCurve = AnimationCurve.Linear(0, 0.1F, 1, 1);

        [TitleGroup(MovementGroup)]
        public AIAgent aiAgent;

        [NonSerialized, ShowInInspector, ReadOnly, TitleGroup(MovementGroup)]
        public Vector2 Velocity;

        [TitleGroup(MovementGroup)]
        public float SkinWidth = 0.1F;

        [TitleGroup(MovementGroup)]
        public float MaxSpeed = 10;

        [TitleGroup(MovementGroup)]
        public float YForce = 5;

        [TitleGroup(MovementGroup)]
        public float SpeedMultiplier = 1;

        [TitleGroup(MovementGroup)]
        public float GravityScale = 1;

        private CollisionStatus collisionStatus;
        public InteractionController InteractionController;

        [ShowInInspector, ReadOnly, TitleGroup(MovementGroup)]
        public CollisionStatus CollisionStatus {
            get {
                return collisionStatus;
            }
            private set {
                collisionStatus = value;
            }
        }

        public void Interact() {
            var interactableMask = GameResources.Instance.InteractableMask;
            var bounds = InteractionController.Hitbox.bounds;
            var hit = Physics2D.OverlapBox(bounds.center, bounds.size, 0, interactableMask);
            if (hit == null) {
                return;
            }
            var e = hit.GetComponentInParent<InteractableElement>();
            if (e != null && e.CanInteract(this)) {
                e.Interact(this);
            }
        }

        public float SpeedPercent {
            get {
                return Velocity.magnitude / MaxSpeed;
            }
        }

        public Vector2 GroundPosition {
            get {
                var bounds = Hitbox.bounds;
                var min = bounds.min;
                return new Vector2(bounds.center.x, min.y + SkinWidth);
            }
        }


        private void Start() {
            Motor.Initialize(this);
        }

        private void OnDisable() {
            Motor.Terminate(this);
        }

        private void Reset() {
            Motor = GetComponent<Motor>();
        }

        private void LateUpdate() {
            if (Motor == null) {
                return;
            }

            Motor.Execute(this, ref collisionStatus);
            transform.position += (Vector3) Velocity * Time.deltaTime;
            var newDirection = Direction.FromVector(Velocity);
            var xDir = newDirection.X;
            var yDir = newDirection.Y;
            if (xDir != 0 && CurrentDirection.X != xDir) {
                CurrentDirection.X = xDir;
            }

            if (yDir != 0 && CurrentDirection.Y != yDir) {
                CurrentDirection.Y = yDir;
            }
        }
    }

    public struct CollisionStatus
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;

        public int HorizontalCollisionDir
        {
            get
            {
                if (Left == Right)
                {
                    return 0;
                }
                if (Left)
                {
                    return -1;
                }
                if (Right)
                {
                    return 1;
                }
                return 0;
            }
        }
    }
}