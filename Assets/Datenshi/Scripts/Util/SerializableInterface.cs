using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Datenshi.Scripts.Util {
    [Serializable]
    public class SerializableInterface<T> where T : class {
        public SerializableInterface() { }

        public SerializableInterface(T o) {
            Object = o as Object;
        }

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

        public static implicit operator T(SerializableInterface<T> i) {
            return i.Value;
        }

        public static implicit operator SerializableInterface<T>(T i) {
            return new SerializableInterface<T>(i);
        }
    }
}