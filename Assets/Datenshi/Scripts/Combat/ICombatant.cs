using System;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Util;
using UPM.Util;
using Datenshi.Scripts.Movement;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Combat {
    public enum CombatRelationship {
        Ally,
        Neutral,
        Enemy
    }

    [Serializable]
    public class SerializableCombatant : SerializableInterface<ICombatant> { }

    public interface ICombatant : ILocatable, IVariableHolder, IInputReceiver {
        uint Damage(ICombatant damageDealer, Attack attack, float multiplier = 1);

        uint MaxHealth {
            get;
            set;
        }

        float HealthPercentage {
            get;
            set;
        }

        void Kill();

        void Heal(uint healthAmount);


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

        CombatantAnimatorUpdater AnimatorUpdater {
            get;
        }


        float FocusTimeLeft {
            get;
            set;
        }

        bool Dead {
            get;
        }

        bool Focusing {
            get;
            set;
        }

        bool CanFocus {
            get;
        }

        bool Stunned {
            get;
        }

        bool Invulnerable {
            get;
            set;
        }

        void Stun(float duration);

        void ExecuteAttack(Attack attack);

        UnityEvent OnKilled {
            get;
        }
    }
}