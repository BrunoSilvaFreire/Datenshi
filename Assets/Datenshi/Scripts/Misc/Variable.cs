using System;

namespace Datenshi.Scripts.Misc {
    public class Variable : IComparable<Variable> {
        public Variable(string key) {
            Key = key;
        }

        public string Key {
            get;
            private set;
        }

        public int CompareTo(Variable other) {
            if (ReferenceEquals(this, other))
                return 0;
            return ReferenceEquals(null, other) ? 1 : string.Compare(Key, other.Key, StringComparison.Ordinal);
        }

        protected bool Equals(Variable other) {
            return string.Equals(Key, other.Key);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((Variable) obj);
        }

        public override int GetHashCode() {
            return (Key != null ? Key.GetHashCode() : 0);
        }

        public static bool operator ==(Variable a, Variable b) {
            if (a == null && b == null) {
                return true;
            }
            if (a == null || b == null) {
                return false;
            }
            return a.Key == b.Key;
        }

        public static bool operator !=(Variable a, Variable b) {
            return !(a == b);
        }
    }

    public sealed class Variable<T> : Variable {
        public T DefaultValue {
            get;
            private set;
        }

        public Variable(string key, T defaultValue) : base(key) {
            DefaultValue = defaultValue;
        }
    }
}