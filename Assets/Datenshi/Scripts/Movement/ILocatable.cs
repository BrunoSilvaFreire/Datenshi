using UnityEngine;

namespace Datenshi.Scripts.Movement {
    public interface ILocatable {
        Transform Transform {
            get;
        }

        Vector2 Center {
            get;
        }

        Vector2 GroundPosition {
            get;
        }
    }
}