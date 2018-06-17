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
        public float DamageLowfilterDefault = 0;
        public float LowCutoff = 440;

        private IEnumerator DoFreezeTimeCombo(float timeScale, float timeScaleDuration, ICombatant entity) {
            TimeController.Instance.Slowdown(timeScale, timeScaleDuration);
            AudioManager.Instance.ImpactHighFilter(DamageCutoff, LowCutoff, timeScaleDuration);
            var animator = entity.AnimatorUpdater.Animator;
            var bnw = GraphicsSingleton.Instance.BlackAndWhite;
            bnw.DoAmountImpact(1, timeScaleDuration);
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            yield return new WaitForSeconds(timeScaleDuration);
            animator.updateMode = AnimatorUpdateMode.Normal;
        }


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
        }
    }
}