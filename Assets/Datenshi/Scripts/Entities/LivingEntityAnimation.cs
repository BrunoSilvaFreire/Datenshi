using Datenshi.Scripts.Combat;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity {
        [SerializeField]
        private CombatantAnimatorUpdater updater;

        public CombatantAnimatorUpdater AnimatorUpdater {
            get {
                return updater;
            }
            protected set {
                updater = value;
            }
        }
    }
}