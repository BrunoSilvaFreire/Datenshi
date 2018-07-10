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

    public interface IDamageable {
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

        uint Damage(ICombatant damageDealer, ref DamageInfo damageInfo, IDefendable defendable = null);
    }

    public interface ICombatant : ILocatable, IVariableHolder, IInputReceiver, IDamageable {
        FloatVolatileProperty DamageMultiplier {
            get;
        }


        Direction CurrentDirection {
            get;
            set;
        }

        bool Ignored {
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

        void ExecuteAttack(Attack attack);

        UnityEvent OnKilled {
            get;
        }
    }
}