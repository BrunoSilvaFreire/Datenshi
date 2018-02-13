using UnityEngine;

namespace Datenshi.Scripts {
    public static class Constants {
        public const float Gravity = -19.62F;

        //How many points a link (GravityLink) should have per meter at velocity of magnitude 1
        public const float Precision = 0.5F;

        public const float NavmeshBoxcastDownsizeValue = 0.1F;
        public static readonly Vector2 NavmeshBoxcastDownsizeScale = new Vector2(NavmeshBoxcastDownsizeValue, NavmeshBoxcastDownsizeValue);

        public const float MinPrecision = 0.1F;
        public const float MaxPrecision = 100;
        public const string PrecisionConfigKey = "datenshi.navmesh.precision";

        public const float DefaultSpeed = 10;
        public const float DefaultJumpHeight = 10;

        public const float DefaultInputThreshold = 0.1F;
    }
}