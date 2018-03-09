using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Controller {
    [CreateAssetMenu(menuName = "Datenshi/Resources/GameResources")]
    public class GameResources : SingletonScriptableObject<GameResources> {
        public LayerMask WorldMask;
        public Camera CharacterCameraPrefab;
        public LayerMask EntitiesMask;
        public float Gravity = -19.62F;
        public float NavmeshBoxcastDownsizeValue = 0.1F;

        public float MinPrecision = 1F;
        public float MaxPrecision = 100;
        public float DefaultPrecision = 0.5F;
        public const string PrecisionConfigKey = "datenshi.navmesh.precision";

        public Vector2 NavmeshBoxcastDownsizeScale {
            get {
                return new Vector2(NavmeshBoxcastDownsizeValue, NavmeshBoxcastDownsizeValue);
            }
        }
    }
}