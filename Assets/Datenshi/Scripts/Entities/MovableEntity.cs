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

        [TitleGroup(MovementGroup)]
        public AnimationCurve AccelerationCurve = AnimationCurve.Linear(0, 0.1F, 1, 1);

        [TitleGroup(MovementGroup)]
        public AIAgent AIAgent;

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

        [TitleGroup(GeneralGroup)]
        public InteractionController InteractionController;

        [TitleGroup(MovementGroup)]
        public bool ApplyVelocity = true;

        [TitleGroup(CombatGroup)]
        public bool DamageGivesKnockback;

        [ShowIf("DamageGivesKnockback"), TitleGroup(CombatGroup)]
        public uint DamageKnockbackMin = 10;

        [ShowIf("DamageGivesKnockback"), TitleGroup(CombatGroup)]
        public float DamageKnockbackStrenght = 3;

        [ShowIf("DamageGivesKnockback"), TitleGroup(CombatGroup)]
        public float KnockbackLiftoff = 3;

        [TitleGroup(MovementGroup)]
        public Vector2 ExternalForces;

        [TitleGroup(MovementGroup)]
        public float ExternalForcesDeacceleration = 0.1F;

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
            if (Stunned) {
                return;
            }
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
            if (ApplyVelocity && !Stunned) {
                transform.position += (Vector3) Velocity * Time.deltaTime;
            }
            if (ExternalForces.magnitude > 0.1) {
                PhysicsUtil.DoPhysics(this, ref ExternalForces, ref collisionStatus);
                transform.position += (Vector3) ExternalForces;
                ExternalForces = Vector2.Lerp(ExternalForces, Vector2.zero, ExternalForcesDeacceleration);
            }
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

        public override void Damage(LivingEntity entity, uint damage) {
            base.Damage(entity, damage);
            if (DamageGivesKnockback && damage >= DamageKnockbackMin) {
                ExternalForces = transform.position - entity.transform.position;
                ExternalForces.Normalize();
                ExternalForces *= DamageKnockbackStrenght;
                if (ExternalForces.y <= 0) {
                    ExternalForces.y += KnockbackLiftoff;
                }
            }
        }

        
    }

    public struct CollisionStatus {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;

        public int HorizontalCollisionDir {
            get {
                if (Left == Right) {
                    return 0;
                }
                if (Left) {
                    return -1;
                }
                if (Right) {
                    return 1;
                }
                return 0;
            }
        }
    }
}