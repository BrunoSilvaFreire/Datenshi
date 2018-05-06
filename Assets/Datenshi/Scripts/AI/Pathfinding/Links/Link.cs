namespace Datenshi.Scripts.AI.Pathfinding.Links {
    public abstract class Link {
        public abstract void Execute(INavigable entity, DummyInputProvider provider, Navmesh navmesh);
        public abstract bool CanMakeIt(INavigable entity);
        public abstract int GetOrigin();
        public abstract int GetDestination();
#if UNITY_EDITOR
        public abstract bool DrawOnlyOnMouseOver();
        public abstract void DrawGizmos(Navmesh navmesh, uint originNodeIndex, float precision, bool precisionChanged);
#endif
    }
}