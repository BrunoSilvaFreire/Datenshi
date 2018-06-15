using Datenshi.Scripts.Combat;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity {
        [SerializeField]
        private CombatantAnimatorUpdater updater;

        public CombatantAnimatorUpdater AnimatorUpdater => updater;
    }
}