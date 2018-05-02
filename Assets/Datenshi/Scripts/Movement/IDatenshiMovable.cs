using UPM.Motors;

namespace Datenshi.Scripts.Movement {
    public interface IDatenshiMovable : ILocatable, IMovable {
        float SpeedMultiplier {
            get;
            set;
        }
    }
}