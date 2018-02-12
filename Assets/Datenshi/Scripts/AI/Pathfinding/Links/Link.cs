namespace Datenshi.Scripts.AI.Pathfinding.Links {
    public abstract class Link {
        public abstract void Execute();
#if UNITY_EDITOR
        public abstract void DrawGizmos(Navmesh navmesh);
#endif
    }
}