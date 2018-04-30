using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;

namespace Datenshi.Scripts.AI.Pathfinding.Links {
    public abstract class Link {
        public abstract void Execute(MovableEntity entity, AIStateInputProvider provider, Navmesh navmesh);
        public abstract bool CanMakeIt(MovableEntity entity);
        public abstract int GetOrigin();
        public abstract int GetDestination();
#if UNITY_EDITOR
        public abstract bool DrawOnlyOnMouseOver();
        public abstract void DrawGizmos(Navmesh navmesh, uint originNodeIndex, float precision, bool precisionChanged);
#endif
    }
}