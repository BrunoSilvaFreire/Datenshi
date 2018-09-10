using Datenshi.Scripts.Util.Volatiles;
using UnityEngine;
using UPM.Motors;

namespace Datenshi.Scripts.Movement {
    public interface IDatenshiMovable : ILocatable, IMovable {

        FloatVolatileProperty SpeedMultiplier {
            get;
        }

        Vector2 ExternalForces {
            get;
            set;
        }
     
        bool ApplyVelocity {
            get;
            set;
        }

        bool TimeScaleIndependent {
            get;
            set;
        }

        float DeltaTime {
            get;
        }

    }
}