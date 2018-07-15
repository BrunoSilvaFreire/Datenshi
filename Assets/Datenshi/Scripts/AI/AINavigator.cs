using Datenshi.Scripts.AI.Pathfinding;
using Datenshi.Scripts.Movement;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Profiling;

namespace Datenshi.Scripts.AI {
    public abstract class AINavigator : MonoBehaviour {
        public abstract Vector2 SetTarget(Vector2 target);
        public abstract Vector2 GetTarget();

        [ShowInInspector, ReadOnly]
        private float timeSinceLastRegen;

        public float TimePerRegen = 1;

        private void Update() {
            if (timeSinceLastRegen < TimePerRegen) {
                timeSinceLastRegen += Time.deltaTime;
            } else if (CanReload()) {
                Profiler.BeginSample("Path Reload @ " + GetType().Name);
                ReloadPath();
                Profiler.EndSample();
                timeSinceLastRegen = 0;
            }
        }

        protected abstract bool CanReload();

        protected abstract void ReloadPath();

        public abstract void Execute(INavigable navigable, DummyInputProvider provider);

        public abstract Vector2 GetFavourablePosition(ILocatable target);
        public abstract Vector2 GetFavourablePosition(Vector2 targetPos);
        public abstract bool IsValid(Node node);

        public abstract Vector2 GetFavourableStartPosition(INavigable navigable);
    }
}