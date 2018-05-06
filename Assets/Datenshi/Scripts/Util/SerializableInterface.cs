using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Datenshi.Scripts.Util {
    [Serializable]
    public class SerializableInterface<T> where T : class {
        [ShowInInspector]
        public T Value {
            get {
                return Object as T;
            }
            set {
                Object = value as Object;
            }
        }


        [SerializeField, ReadOnly]
        private Object Object;
    }
}