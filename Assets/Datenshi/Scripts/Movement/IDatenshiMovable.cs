using Datenshi.Scripts.Util;
using UPM.Motors;

namespace Datenshi.Scripts.Movement {
    public interface IDatenshiMovable : ILocatable, IMovable {
        float BaseSpeedMultiplier {
            get;
            set;
        }

        float SpeedMultiplier {
            get;
        }

        Direction Direction {
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