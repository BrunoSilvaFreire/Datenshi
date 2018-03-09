using Datenshi.Scripts.Combat.Strategies;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.AI {
    public abstract class AIAgent : MonoBehaviour {
        public Vector2 Target;

        [ShowInInspector, ReadOnly]
        private float timeSinceLastRegen;

        public float TimePerRegen = 1;

        private void Update() {
            if (timeSinceLastRegen < TimePerRegen) {
                timeSinceLastRegen += Time.deltaTime;
            } else if (CanReload()) {
                ReloadPath();
                timeSinceLastRegen = 0;
            }
        }

        protected abstract bool CanReload();

        protected abstract void ReloadPath();

        public abstract void Execute(MovableEntity entity, AIStateInputProvider provider);

        public abstract Vector2 GetFavourablePosition(RangedAttackStrategy state, LivingEntity target);
    }
}