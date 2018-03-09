using Datenshi.Scripts.AI.Pathfinding;
using UnityEngine;

namespace Datenshi.Scripts.World {
    [CreateAssetMenu(menuName = "Datenshi/World/Area")]
    public class Area : ScriptableObject {
        public Navmesh Navmesh;
    }
}