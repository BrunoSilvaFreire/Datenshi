using System;
using System.Collections.Generic;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Combat.Status;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Util;
using UPM.Util;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.Util.Volatiles;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Combat {
    public enum CombatRelationship {
        Ally,
        Neutral,
        Enemy
    }

    [Serializable]
    public class SerializableCombatant : SerializableInterface<ICombatant> {
        public SerializableCombatant() { }
        public SerializableCombatant(ICombatant o) : base(o) { }
    }

    [Serializable]
    public class SerializableDamageDealer : SerializableInterface<IDamageDealer> {
        public SerializableDamageDealer() { }
        public SerializableDamageDealer(IDamageDealer o) : base(o) { }
    }

    [Serializable]
    public class SerializableDamageable : SerializableInterface<IDamageable> {
        public SerializableDamageable() { }
        public SerializableDamageable(IDamageable o) : base(o) { }
    }

    [Serializable]
    public class CombatantDamagedEvent : UnityEvent<DamageInfo> { }

    /// <summary>
    /// Representa uma fonte de dano.
    /// <br/>
    /// </summary>
    public interface IDamageSource {
        uint GetDamage(IDamageable damageable);
    }

    /// <summary>
    /// Algo que pode dar dano em um <see cref="IDamageable"/> utilizando um <see cref="IDamageSource"/>.
    /// </summary>
    public interface IDamageDealer : ILocatable {
        FloatVolatileProperty DamageMultiplier {
            get;
        }
    }

    /// <summary>
    /// Algo que pode receber dano de um <see cref="IDamageDealer"/> através de um <see cref="IDamageSource"/>
    /// </summary>
    public interface IDamageable : ILocatable {
        bool Ignored {
            get;
            set;
        }

        uint MaxHealth {
            get;
            set;
        }

        float HealthPercentage {
            get;
            set;
        }

        bool Dead {
            get;
        }

        bool GodMode {
            get;
            set;
        }

        void Kill();

        void Heal(uint healthAmount);

        uint Damage(ref DamageInfo damageInfo, IDefendable defendable = null);

        CombatantDamagedEvent OnDamaged {
            get;
        }

        UnityEvent OnKilled {
            get;
        }
    }

    public interface ICombatant : IVariableHolder, IInputReceiver, IDamageable, IDamageDealer {
        Direction CurrentDirection {
            get;
            set;
        }


        Bounds2D DefenseHitbox {
            get;
        }

        Collider2D Hitbox {
            get;
        }

        CombatRelationship Relationship {
            get;
        }

        void SetInvulnerable(float duration);

        CombatantAnimatorUpdater AnimatorUpdater {
            get;
        }


        float FocusTimeLeft {
            get;
            set;
        }

        bool CanFocus {
            get;
        }

        bool Focusing {
            get;
            set;
        }

        bool Defending {
            get;
            set;
        }

        bool Stunned {
            get;
        }


        void Stun(float duration);

        void ExecuteSkill(ActiveSkill skill);
    }
}