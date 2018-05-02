using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Data {
    public class Ownable<O> : MonoBehaviour {
        [ShowInInspector, ReadOnly]
        private O owner;

        public O Owner {
            get {
                return owner;
            }
            protected set {
                owner = value;
            }
        }
    }
}