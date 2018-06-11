using System.Collections.Generic;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UPM;
using UPM.Input;
using UPM.Motors;
using UPM.Motors.Config;

namespace Datenshi.Scripts.Entities {
    /// <summary>
    /// Uma entidade que se move.
    /// <br />
    /// A maneira com que ela se move depende de seu <see cref="F:Datenshi.Scripts.Entities.MovableEntity.Motor" />.
    /// </summary>
    public class MovableEntity : LivingEntity, INavigable, IDatenshiMovable {
        public const string MovementGroup = "Movement";

        /// <summary>
        /// O motor a ser utilizado por essa entidade para movimentação
        /// </summary>
        [SerializeField, HideInInspector]
        private Motor motor;

        [TitleGroup(MovementGroup), SerializeField]
        private AnimationCurve accelerationCurve = AnimationCurve.Constant(0, 1, 1);

        [SerializeField, HideInInspector]
        private AINavigator aiNavigator;

        [ShowInInspector, TitleGroup(MovementGroup)]
        public AINavigator AINavigator {
            get {
                return aiNavigator;
            }
            set {
                aiNavigator = value;
            }
        }


        [TitleGroup(MovementGroup)]
        public float SkinWidth = 0.1F;

        [TitleGroup(MovementGroup), SerializeField]
        private float maxSpeed = 10;

        [TitleGroup(MovementGroup)]
        public float YForce = 5;

        [SerializeField, TitleGroup(MovementGroup)]
        private float baseSpeedMultiplier = 1;


        public float BaseSpeedMultiplier {
            get {
                return baseSpeedMultiplier;
            }
            set {
                baseSpeedMultiplier = value;
            }
        }

        public float SpeedMultiplier {
            get {
                var baseSpeed = baseSpeedMultiplier;

                return baseSpeed;
            }
        }


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


        public C GetMotorConfig<C>() where C : MotorConfig {
            return motorConfig as C;
        }

        public InputProvider InputProvider => base.InputProvider;

        public AnimationCurve AccelerationCurve => accelerationCurve;

        public Collider2D Hitbox => base.Hitbox;


        [SerializeField]
        private float inset = .05F;

        [SerializeField]
        private byte horizontalRaycasts = 3;

        [SerializeField]
        private byte verticalRaycasts = 3;

        public float Inset => inset;

        public byte HorizontalRaycasts => horizontalRaycasts;

        public byte VerticalRaycasts => verticalRaycasts;

        public float MaxSpeed => maxSpeed;

        public Transform MovementTransform => transform;

        [TitleGroup(CombatGroup)]
        public bool StunRemovesVelocity;

        [TitleGroup(CombatGroup), ShowIf("StunRemovesVelocity")]
        public float StunVelocityDampen = 1;

        [ShowInInspector, ReadOnly, TitleGroup(MovementGroup)]
        public Vector2 Velocity {
            get;
            set;
        }

        [ShowInInspector, ReadOnly, TitleGroup(MovementGroup)]
        public CollisionStatus CollisionStatus {
            get {
                return collisionStatus;
            }
            set {
                collisionStatus = value;
            }
        }


        public float SpeedPercent => Velocity.magnitude / MaxSpeed;

        public Vector2 GroundPosition {
            get {
                var bounds = Hitbox.bounds;
                var min = bounds.min;
                return new Vector2(bounds.center.x, min.y + SkinWidth);
            }
        }

        public Direction Direction {
            get {
                return direction;
            }
            set {
                direction = value;
            }
        }

        [ShowInInspector, TitleGroup(MovementGroup, "Informações sobre a maneira de locomoção desta entidade")]
        public Motor Motor {
            get {
                return motor;
            }
            set {
                motor = value;
                if (value == null || !motor.RequiresConfig(this)) {
                    return;
                }

                var config = MotorConfig;
                if (config != null && config.IsCompatible(motor)) {
                    return;
                }

                var foundConfig = GetComponentInChildren<MotorConfig>();
                var foundValid = foundConfig != null && foundConfig.IsCompatible(motor);
                MotorConfig = foundValid ? foundConfig : value.CreateConfig(this);
            }
        }

        public void Move() {
            motor.Move(this);
            if (ApplyVelocity) {
                MovementTransform.position += (Vector3) Velocity * DeltaTime;
            }
        }

        protected override void Update() {
            base.Update();
            UpdateMovement();
            UpdateDirection();
        }

        private void UpdateDirection() {
            var newDirection = Direction.FromVector(Velocity);
            var xDir = newDirection.X;
            var yDir = newDirection.Y;
            var dir = Direction;
            if (xDir != 0 && Direction.X != xDir) {
                dir.X = xDir;
            }

            if (yDir != 0 && Direction.Y != yDir) {
                dir.Y = yDir;
            }

            Direction = dir;
        }

        private void UpdateMovement() {
            if (ExternalForces.magnitude > 0.1) {
                ExternalForces = Vector2.Lerp(ExternalForces, Vector2.zero, ExternalForcesDeacceleration);
                Velocity += ExternalForces * DeltaTime;
            }

            Move();
        }


        [SerializeField, HideInInspector]
        private MotorConfig motorConfig;

        [SerializeField]
        private CollisionStatus collisionStatus;

        [ShowInInspector, TitleGroup(MovementGroup)]
        public MotorConfig MotorConfig {
            get {
                return motorConfig;
            }

            set {
                if (value != null) {
                    var m = motor;
                    if (m != null && m.RequiresConfig(this) && !value.IsCompatible(m)) {
                        Debug.LogWarning("Attempted to set motor config " + value + " incompatible to motor " + motor);
                        return;
                    }
                } else if (motor.RequiresConfig(this)) {
                    Debug.LogWarning("Attempted to remove motor config required by motor " + motor);
                    return;
                }

                motorConfig = value;
            }
        }

        public bool ApplyVelocity {
            get;
            set;
        } = true;

        [SerializeField]
        private bool timeScaleIndependent;

        public bool TimeScaleIndependent {
            get {
                return timeScaleIndependent;
            }
            set {
                timeScaleIndependent = value;
            }
        }

        public float DeltaTime => TimeScaleIndependent ? Time.unscaledDeltaTime : Time.deltaTime;
        private List<SpeedEffector> speedEffectors = new List<SpeedEffector>();

        private struct SpeedEffector {
            private readonly float magnitude;
            private readonly float duration;
            private float timeLeft;

            public SpeedEffector(float magnitude, float duration) : this() {
                this.magnitude = magnitude;
                this.duration = duration;
                timeLeft = duration;
            }

            public bool Tick() {
                timeLeft -= Time.deltaTime;
                return timeLeft <= 0;
            }
        }

        public void AddSpeedEffector(float magnitude, float duration) {
            //TODO implement
        }

        public override void Stun(float duration) {
            base.Stun(duration);
            if (GodMode || !StunRemovesVelocity) {
                return;
            }

            Velocity *= 1 - StunVelocityDampen;
        }

        public override uint Damage(ICombatant entity, ref DamageInfo info, IDefendable defendable = null) {
            var damage = base.Damage(entity, ref info, defendable);
            if (!DamageGivesKnockback || damage < DamageKnockbackMin) {
                return damage;
            }

            ExternalForces = Center - entity.Center;
            ExternalForces.Normalize();
            ExternalForces *= DamageKnockbackStrenght;
            if (ExternalForces.y <= 0) {
                ExternalForces.y += KnockbackLiftoff;
            }

            return damage;
        }
    }
}