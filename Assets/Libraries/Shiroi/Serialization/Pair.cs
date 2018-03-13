using System;
using UnityEngine;

namespace Shiroi.Serialization {
    public abstract class Pair<T> : IComparable<string> {
        protected Pair() { }

        protected Pair(string key, T value) {
            this.key = key;
            this.value = value;
        }

        [SerializeField] private string key;
        [SerializeField] private T value;

        public string Key {
            get { return key; }
        }

        public T Value {
            get { return value; }
            set { this.value = value; }
        }

        public int CompareTo(string other) {
            return string.Compare(key, other, StringComparison.OrdinalIgnoreCase);
        }
    }
}