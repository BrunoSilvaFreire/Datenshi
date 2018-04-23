using Datenshi.Scripts.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class Ownable : MonoBehaviour {
        [ShowInInspector, ReadOnly]
        private LivingEntity owner;

        public LivingEntity Owner {
            get {
                return owner;
            }
            protected set {
                owner = value;
            }
        }
    }
}