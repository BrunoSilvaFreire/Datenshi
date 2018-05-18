using Datenshi.Scripts.Util.Singleton;
using UnityEngine;

namespace Datenshi.Scripts.Data {
    [CreateAssetMenu(menuName = "Datenshi/Resources/GameResources")]
    public class GameResources : SingletonScriptableObject<GameResources> {
        public LayerMask WorldMask;
        public LayerMask EntitiesMask;
        public LayerMask InteractableMask;
        public Camera CharacterCameraPrefab;
        public float Gravity = -19.62F;
        public float NavmeshBoxcastDownsizeValue = 0.1F;

        public float MinPrecision = 1F;
        public float MaxPrecision = 100;
        public float DefaultPrecision = 0.5F;
        public const string PrecisionConfigKey = "datenshi.navmesh.precision";

        public Vector2 NavmeshBoxcastDownsizeScale => new Vector2(NavmeshBoxcastDownsizeValue, NavmeshBoxcastDownsizeValue);

        public float DeflectSpeed = 20;
        public float DeflectDamageMultiply = 3;
    }
}