using System;

namespace Datenshi.Scripts.Data {
    public interface IVariableHolder {
        void SetVariable<T>(Variable<T> variable, T value);
        T GetVariable<T>(Variable<T> variable);
    }

    public class Variable : IEquatable<Variable>, IComparable<Variable> {
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


        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return Equals((Variable) obj);
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

        public bool Equals(Variable other) {
            if (ReferenceEquals(null, other))
                return false;
            return ReferenceEquals(this, other) || string.Equals(Key, other.Key);
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