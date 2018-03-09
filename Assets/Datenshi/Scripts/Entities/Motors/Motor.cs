using UnityEngine;

namespace Datenshi.Scripts.Entities.Motors {
    /// <summary>
    /// Uma maneira de se movimentar, seja pelo chão, voando, etc.
    /// Quando a entidade for atualizar sua movimentação (a cada Update), ela chamará o <see cref="Execute"/>.
    /// Ela também irá chamar <see cref="Initialize"/> e <see cref="Terminate"/> quando for iniciada e destruida.
    /// </summary>
    public abstract class Motor : ScriptableObject {
        public uint HorizontalRays = 3;
        public uint VerticalRays = 3;
        public float MaxAngle = 50;

        /// <summary>
        /// Chamado no <code>Start()</code> da <see cref="entity"/>.    
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Initialize(MovableEntity entity);

        /// <summary>
        /// Chamado a cada <code>Update()</code> da <see cref="entity"/>.
        /// Implementações devem atualizar o <see cref="collStatus"/>, para uso em outros sistemas.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="collStatus"></param>
        public abstract void Execute(MovableEntity entity, ref CollisionStatus collStatus);

        /// <summary>
        /// Chamado no <code>OnDisable()</code> da <see cref="entity"/>.
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Terminate(MovableEntity entity);
    }

    public abstract class Motor<C> : Motor where C : MotorConfig {
        public override void Initialize(MovableEntity entity) {
            Initialize(entity, entity.Config as C);
        }

        public override void Execute(MovableEntity entity, ref CollisionStatus collStatus) {
            Execute(entity, entity.Config as C, ref collStatus);
        }

        public override void Terminate(MovableEntity entity) {
            Terminate(entity, entity.Config as C);
        }

        /// <summary>
        /// Chamado no <code>Start()</code> da <see cref="entity"/>.    
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Initialize(MovableEntity entity, C config);

        /// <summary>
        /// Chamado a cada <code>Update()</code> da <see cref="entity"/>.
        /// Implementações devem atualizar o <see cref="collStatus"/>, para uso em outros sistemas.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="collStatus"></param>
        public abstract void Execute(MovableEntity entity, C config, ref CollisionStatus collStatus);

        /// <summary>
        /// Chamado no <code>OnDisable()</code> da <see cref="entity"/>.
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Terminate(MovableEntity entity, C config);
    }
}