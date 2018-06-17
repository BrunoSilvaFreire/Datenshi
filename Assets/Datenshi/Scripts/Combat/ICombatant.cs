using System;
using System.Collections;
using System.Collections.Generic;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Combat.Status;
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
    public class SerializableCombatant : SerializableInterface<ICombatant> {
        public SerializableCombatant() { }
        public SerializableCombatant(ICombatant o) : base(o) { }
    }

    public interface ICombatant : ILocatable, IVariableHolder, IInputReceiver {
        uint Damage(ICombatant damageDealer, ref DamageInfo damageInfo, IDefendable defendable = null);

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

        void SetInvulnerable(float duration);

        CombatantAnimatorUpdater AnimatorUpdater {
            get;
        }


        float DefendTimeLeft {
            get;
            set;
        }

        bool Dead {
            get;
        }

        bool Defending {
            get;
            set;
        }

        bool CanDefend {
            get;
        }

        bool Stunned {
            get;
        }

        bool GodMode {
            get;
            set;
        }

        void Stun(float duration);

        void ExecuteAttack(Attack attack);

        UnityEvent OnKilled {
            get;
        }

        Coroutine StartCoroutine(IEnumerator evasiveDash);
    }
}