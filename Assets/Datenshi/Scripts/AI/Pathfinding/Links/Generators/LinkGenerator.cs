using System.Collections.Generic;
using UnityEngine;

namespace Datenshi.Scripts.AI.Pathfinding.Links.Generators {
    public static class LinkGenerators {
        public static readonly LinkGenerator[] Generators = {
            new LinearLinkGenerator(),
            new GravityLinkGenerator(),
        };
    }

    public abstract class LinkGenerator {
        public abstract IEnumerable<Link> Generate(Node node, Navmesh navmesh, Vector2 nodeWorldPos, float precision);
#if UNITY_EDITOR
        public abstract void DrawEditor();
#endif
    }
}