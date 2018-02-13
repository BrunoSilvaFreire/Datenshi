namespace Datenshi.Scripts.AI.Pathfinding.Links {
    public abstract class Link {
        public abstract void Execute();
#if UNITY_EDITOR
        public abstract bool DrawOnlyOnMouseOver();
        public abstract void DrawGizmos(Navmesh navmesh, uint originNodeIndex, float precision, bool precisionChanged);
#endif
    }
}