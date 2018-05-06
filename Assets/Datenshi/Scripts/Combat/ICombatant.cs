using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Util;
using UPM.Util;
using Datenshi.Scripts.Movement;

namespace Datenshi.Scripts.Combat {
    public enum CombatRelationship {
        Ally,
        Neutral,
        Enemy
    }

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
    }

}