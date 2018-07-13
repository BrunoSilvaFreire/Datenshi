using System.Collections.Generic;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Volatiles;
using Sirenix.OdinInspector;
using UnityEngine;
using UPM;
using UPM.Input;
using UPM.Motors;
using UPM.Motors.Config;
using UPM.Physics;

namespace Datenshi.Scripts.Entities {
    /// <summary>
    /// Uma entidade que se move.
    /// <br />
    /// A maneira com que ela se move depende de seu <see cref="F:Datenshi.Scripts.Entities.MovableEntity.Motor" />.
    /// </summary>
    public partial class MovableEntity : LivingEntity, INavigable, IDatenshiMovable {
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

        public float DirectionChangeThreshold = .2F;

        [SerializeField]
        private FloatVolatileProperty speedMultiplier;

        public FloatVolatileProperty SpeedMultiplier => speedMultiplier;


        [TitleGroup(CombatGroup)]
        public bool DamageGivesKnockback;

        [ShowIf("DamageGivesKnockback"), TitleGroup(CombatGroup)]
        public uint DamageKnockbackMin = 10;

        [ShowIf("DamageGivesKnockback"), TitleGroup(CombatGroup)]
        public float DamageKnockbackStrenght = 3;

        [ShowIf("DamageGivesKnockback"), TitleGroup(CombatGroup)]
        public float KnockbackLiftoff = 3;

        [TitleGroup(MovementGroup), SerializeField]
        private Vector2 externalForces;

        [TitleGroup(MovementGroup)]
        public float ExternalForcesDeacceleration = 0.1F;


        public C GetMotorConfig<C>() where C : MotorConfig {
            return motorConfig as C;
        }

        [TitleGroup(GeneralGroup)]
        public InputProvider InputProvider => base.InputProvider;

        [TitleGroup(MovementGroup)]
        public AnimationCurve AccelerationCurve => accelerationCurve;


        [TitleGroup(GeneralGroup), SerializeField]
        private float inset = .05F;

        [TitleGroup(MovementGroup), SerializeField]
        private byte horizontalRaycasts = 3;

        [TitleGroup(MovementGroup), SerializeField]
        private byte verticalRaycasts = 3;

        public float Inset => inset;

        public byte HorizontalRaycasts => horizontalRaycasts;

        public byte VerticalRaycasts => verticalRaycasts;

        public float MaxSpeed => maxSpeed * SpeedMultiplier.Value;

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


        public float SpeedPercent {
            get {
                if (MaxSpeed > 0) {
                    return Velocity.x / MaxSpeed;
                }

                return 0;
            }
        }

        public override Vector2 GroundPosition {
            get {
                var bounds = Hitbox.bounds;
                var min = bounds.min;
                return new Vector2(bounds.center.x, min.y + SkinWidth);
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

        private PhysicsBehaviour externalForceBehaviour = new PhysicsBehaviour(
            new HorizontalPhysicsCheck(),
            new VerticalPhysicsCheck()
        );

        public void Move() {
            motor.Move(this);
            if (!ApplyVelocity) {
                return;
            }

            MovementTransform.position += (Vector3) Velocity * DeltaTime;

            if (ExternalForces.magnitude < 0.1) {
                return;
            }
            externalForces = Vector2.Lerp(externalForces, Vector2.zero, ExternalForcesDeacceleration);
            externalForceBehaviour.Check(this, ref externalForces, ref collisionStatus,
                GameResources.Instance.WorldMask);
            MovementTransform.position += (Vector3) externalForces * DeltaTime;
        }

        protected override void Update() {
            base.Update();
            UpdateMovableVariables();
            UpdateMovement();
            UpdateDirection();
        }

        private void UpdateMovableVariables() {
            SpeedMultiplier.Tick();
        }

        private void UpdateDirection() {
            if (Defending && DefendingFor > DirectionChangeThreshold) {
                return;
            }

            var newDirection = Direction.FromVector(Velocity);
            var xDir = newDirection.X;
            var yDir = newDirection.Y;
            var dir = CurrentDirection;
            if (xDir != 0 && dir.X != xDir) {
                dir.X = xDir;
            }

            if (yDir != 0 && dir.Y != yDir) {
                dir.Y = yDir;
            }

            CurrentDirection = dir;
        }

        private void UpdateMovement() {
            if (RuntimeResources.Instance.Paused) {
                return;
            }

            Move();
        }


        [TitleGroup(MovementGroup), SerializeField, HideInInspector]
        private MotorConfig motorConfig;

        [TitleGroup(MovementGroup), SerializeField]
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

        [TitleGroup(MovementGroup), SerializeField]
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

        public Vector2 ExternalForces {
            get {
                return externalForces;
            }
            set {
                externalForces = value;
            }
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

            externalForces = Center - entity.Center;
            externalForces.Normalize();
            externalForces *= DamageKnockbackStrenght;
            if (externalForces.y <= 0) {
                externalForces.y += KnockbackLiftoff;
            }

            return damage;
        }
    }
}