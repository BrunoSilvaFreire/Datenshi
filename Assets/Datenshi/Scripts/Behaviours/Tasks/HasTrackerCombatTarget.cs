using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Misc;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class HasTrackerCombatTarget : Conditional {
        public CombatStateTracker Tracker;
        public SharedCombatant TargetToSet;
        public bool IgnoreIfHasTarget;

        public override void OnStart() {
            CombatTargetChangedEvent.Instance.AddListener(OnChanged);
        }

        private void OnChanged(LivingEntity arg0, ICombatant arg1) {
            if ((Object) TargetToSet.Value == null && arg1 == Tracker.Combatant) {
                TargetToSet.Value = arg0;
            }
        }

        public override TaskStatus OnUpdate() {
            if (IgnoreIfHasTarget && !IsInvalid(TargetToSet.Value)) {
                return TaskStatus.Success;
            }

            ICombatant targetCandidate = null;
            var dealer = Tracker.LastDealer as ICombatant;
            if (!IsInvalid(dealer)) {
                targetCandidate = dealer;
            } else {
                var attacked = Tracker.LastDamaged as ICombatant;
                if (!IsInvalid(attacked)) {
                    targetCandidate = attacked;
                }
            }

            if ((Object) targetCandidate == null) {
                return TaskStatus.Failure;
            }

            TargetToSet.Value = targetCandidate;
            return TaskStatus.Success;
        }

        private static bool IsInvalid(ICombatant c) {
            return (Object) c == null || c.Dead;
        }
    }
}