using Datenshi.Scripts.Input;
using Datenshi.Scripts.UI.Misc;
using Datenshi.Scripts.Util;
using Rewired;
using Rewired.Utils.Libraries.TinyJson;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Combat.Attacks.UI {
    [SerializeField]
    public class CounterEvent : UnityEvent<ICombatant> {
        public static readonly CounterEvent Instance = new CounterEvent();
        private CounterEvent() { }
    }

    public class HitboxAttackCounterWindow : MonoBehaviour, IQuickTimeEventController, IDefendable {
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

        public void EnableCounterWindow() {
            Available = true;
        }

        public void DisableCounterWindow() {
            Available = false;
        }

        public Vector2 Offset;
        public UIQuickTimeEventElement QTEElementPrefab;

        public string CounterStartKey = "CounterStart";
        public string CounterExitKey = "CounterExit";
        public string CounterSuccessKey = "CounterSucess";

        public bool CanDefend(ICombatant entity) {
            return available;
        }

        public void Defend(ICombatant entity, ref DamageInfo info) {
            info.Canceled = true;
            var updater = entity.AnimatorUpdater;
            var c = GetComponentInParent<ICombatant>();
            if (c == null) {
                return;
            }

            UnityAction onSucess = delegate {
                available = false;
                updater.SetTrigger(CounterSuccessKey);
                c.Kill();
            };
            UnityAction onFailure = delegate { updater.SetTrigger(CounterExitKey); };
            updater.SetTrigger(CounterStartKey);
            var action = ActionsExtensions.GetRandomGameplayAction();
            var a = ReInput.mapping.GetAction((int) action);
            var inverted = false;
            if (a.type == InputActionType.Axis) {
                inverted = RandomUtil.NextBool();
            }

            CounterEvent.Instance.Invoke(c);
            var qte = QTEElementPrefab.Clone(entity.Center + Offset);

            qte.Play(entity.InputProvider, (int) action, inverted, onSucess, onFailure);
        }


        public bool CanPoorlyDefend(ICombatant entity) {
            return available;
        }

        public void PoorlyDefend(ICombatant entity, ref DamageInfo info) {
            Defend(entity, ref info);
        }

        public DefenseType GetDefenseType() {
            return DefenseType.Counter;
        }
    }
}