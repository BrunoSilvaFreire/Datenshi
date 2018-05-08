using Datenshi.Scripts.Input;
using Datenshi.Scripts.UI.Misc;
using Rewired.Utils.Libraries.TinyJson;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Combat.Attacks.UI {
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

        public float TimeDuration = 0.1F;
        public Vector2 Offset;
        public UIQuickTimeEventElement QTEElementPrefab;
        public string StartQuickTimeKey = "Hold";
        public string CounterKey = "Counter";

        public bool CanDefend(ICombatant entity) {
            return available;
        }

        public void Defend(ICombatant entity, ref DamageInfo info) {
            info.Canceled = true;
            UnityAction onSucess = delegate { };
            var e = new QuickTimeEventExecutor(this, TimeDuration, Offset, entity);
            e.Execute(onSucess, null, Actions.Attack, QTEElementPrefab);
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