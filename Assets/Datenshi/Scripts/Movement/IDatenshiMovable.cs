using Datenshi.Scripts.Util;
using UPM.Motors;

namespace Datenshi.Scripts.Movement {
    public interface IDatenshiMovable : ILocatable, IMovable {
        float SpeedMultiplier {
            get;
            set;
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

        void AddSpeedEffector(float magnitude, float duration);
    }
}