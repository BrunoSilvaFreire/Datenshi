using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Util.Volatiles;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Misc {
    public class AbstractDamageDealer : MonoBehaviour, IDamageDealer {
        [SerializeField]
        private FloatVolatileProperty damageMultiplier;

        public Transform Transform => transform;

        public Vector2 Center => transform.position;

        public Vector2 GroundPosition => Center;

        public FloatVolatileProperty DamageMultiplier => damageMultiplier;
    }
}