using System.Collections;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Game;
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

        public bool CanDefend(ICombatant entity) {
            return available && entity != owner;
        }

        public void Defend(ICombatant entity, ref DamageInfo info) {
            info.Canceled = true;
            var c = GetComponentInParent<ICombatant>();
            Debug.Log(entity + " Defending against " + c);
            if (c == null) {
                return;
            }

            c.Stun(StunDuration);
            StartCoroutine(DoFreezeTimeCombo(TimeScale, TimeScaleDuration, entity));
        }

        public float DamageCutoff = 5000;
        public float DamageLowfilterDefault = 0;
        public float LowCutoff = 440;

        private IEnumerator DoFreezeTimeCombo(float timeScale, float timeScaleDuration, ICombatant entity) {
            TimeController.Instance.ImpactFrame(timeScale, timeScaleDuration);
            AudioManager.Instance.ImpactHighFilter(DamageCutoff, LowCutoff, timeScaleDuration);
            var animator = entity.AnimatorUpdater.Animator;
            var bnw = GraphicsSingleton.Instance.BlackAndWhite;
            bnw.DoAmountImpact(1, timeScaleDuration);
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            yield return new WaitForSeconds(timeScaleDuration);
            animator.updateMode = AnimatorUpdateMode.Normal;
        }


        public bool CanPoorlyDefend(ICombatant entity) {
            return false;
        }

        public void PoorlyDefend(ICombatant entity, ref DamageInfo info) { }

        public DefenseType GetDefenseType() {
            return DefenseType.Counter;
        }
    }
}