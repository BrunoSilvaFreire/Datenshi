using System.Collections;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Game.Time;
using Datenshi.Scripts.Graphics;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Combat.Game {
    [SerializeField]
    public class CounterEvent : UnityEvent<ICombatant> {
        public static readonly CounterEvent Instance = new CounterEvent();
        private CounterEvent() { }
    }

    public class HitboxAttackCounterWindow : MonoBehaviour, IDefendable {
        [SerializeField]
        private bool available;

        public bool Available {
            get {
                return available;
            }
            set {
                available = value;
            }
        }

        [UsedImplicitly]
        public void EnableCounterWindow() {
            Available = true;
        }

        [UsedImplicitly]
        public void DisableCounterWindow() {
            Available = false;
        }

        public float TimeScaleDuration;
        public float TimeScale;
        public float StunDuration;
        private ICombatant owner;

        private void OnDrawGizmos() {
            if (!available) {
                return;
            }

            var b = owner.Hitbox.bounds;
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(b.center, b.size);
        }

        private void Start() {
            owner = GetComponentInParent<ICombatant>();
        }

        public float DamageCutoff = 5000;
        public float LowCutoff = 440;
/*


        public bool CanAutoDefend(ICombatant combatant) {
            return true;
        }

        public float DoAutoDefend(ICombatant combatant, ref DamageInfo info) {
            info.Canceled = true;
            return 0;
        }

        public bool CanAgressiveDefend(ICombatant combatant) {
            return available && combatant != owner;
        }

        public float DoAgressiveDefend(ICombatant combatant, ref DamageInfo info) {
            info.Canceled = true;
            var c = GetComponentInParent<ICombatant>();
            Debug.Log(combatant + " Defending against " + c);
            if (c == null) {
                return 0;
            }

            c.Stun(StunDuration);
            StartCoroutine(DoFreezeTimeCombo(TimeScale, TimeScaleDuration, combatant));
            return 0;
        }

        public bool CanEvasiveDefend(ICombatant combatant) {
            return true;
        }

        public float DoEvasiveDefend(ICombatant combatant, ref DamageInfo info) {
            info.Canceled = true;
            return 0;
        }*/
        public bool CanDefend(ICombatant combatant) {
            return true;
        }

        public float Defend(ICombatant combatant, ref DamageInfo info) {
            info.Canceled = true;
            return 0;
        }
    }
}