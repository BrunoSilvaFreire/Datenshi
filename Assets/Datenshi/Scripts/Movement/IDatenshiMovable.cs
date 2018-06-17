using Datenshi.Scripts.Util.Volatiles;
using UPM.Motors;

namespace Datenshi.Scripts.Movement {
    public interface IDatenshiMovable : ILocatable, IMovable {
  

        FloatVolatileProperty SpeedMultiplier {
            get;
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