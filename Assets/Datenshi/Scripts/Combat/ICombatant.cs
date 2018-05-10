using System;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Util;
using UPM.Util;
using Datenshi.Scripts.Movement;
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

        void Damage(ICombatant damageDealer, uint damage);

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