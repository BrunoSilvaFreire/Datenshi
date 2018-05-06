using Unity.Jobs;
using UnityEngine;

namespace Datenshi.Scripts.AI.Jobs {
    public class AerialPathfindingJob : IJob {
        public readonly Vector2 Start;
        public readonly Vector2 End;

        public AerialPathfindingJob(Vector2 start, Vector2 end) {
            Start = start;
            End = end;
        }

        public void Execute() { }
    }
}