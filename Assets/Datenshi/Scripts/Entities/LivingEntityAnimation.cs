using Datenshi.Scripts.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity {
        public const string AnimationGroup = "Animation";

        [SerializeField, BoxGroup(AnimationGroup)]
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