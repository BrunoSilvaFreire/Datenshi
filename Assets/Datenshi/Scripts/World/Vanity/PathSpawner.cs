using Datenshi.Scripts.Util.Misc;
using UnityEngine;

namespace Datenshi.Scripts.World.Vanity {
    public class PathSpawner : PeriodicSpawner<PathExecutorPool, PathExecutor> {
        public Vector3 OriginMaxOffset;
        protected override void OnSpawned(PathExecutor obj) { }
    }
}