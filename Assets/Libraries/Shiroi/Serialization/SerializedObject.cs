using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Serialization {
    [Serializable]
    public sealed class SerializedObject {
        //Lists
        public List<BooleanPair> Booleans = new List<BooleanPair>();

        public List<IntPair> Ints = new List<IntPair>();

        public List<FloatPair> Floats = new List<FloatPair>();

        public List<StringPair> Strings = new List<StringPair>();

        public List<ObjectPair> Objects = new List<ObjectPair>();

        public List<UnityPair> UnityObjects = new List<UnityPair>();
        public List<ArrayPair> Arrays = new List<ArrayPair>();

        //Implementation
        public void SetBoolean(string key, bool value) {
            Upsert(Booleans, key, value, () => new BooleanPair(key, value));
        }

        public void SetInt(string key, int value) {
            Upsert(Ints, key, value, () => new IntPair(key, value));
        }

        public void SetFloat(string key, float value) {
            Upsert(Floats, key, value, () => new FloatPair(key, value));
        }

        public void SetString(string key, string value) {
            Upsert(Strings, key, value, () => new StringPair(key, value));
        }

        public void SetObject(string key, SerializedObject value) {
            Upsert(Objects, key, value, () => new ObjectPair(key, value));
        }

        public void SetUnity(string key, Object value) {
            Upsert(UnityObjects, key, value, () => new UnityPair(key, value));
        }

        public void SetArray(string key, IEnumerable<SerializedObject> value) {
            SetArray(key, value.ToArray());
        }

        public void SetArray(string key, SerializedObject[] value) {
            Upsert(Arrays, key, value, () => new ArrayPair(key, value));
        }

        public bool GetBoolean(string key) {
            BooleanPair first = null;
            foreach (var pair in Booleans) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }

            if (first == null) {
                return NotifyMissing<bool>(key);
            }

            return first.Value;
        }

        private static T NotifyMissing<T>(string key) {
            var value = default(T);
            Debug.LogWarningFormat(
                "[ShiroiCutscenes] Couldn't find {2} for '{0}' when deserializing, returning '{1}' ...",
                key, value, typeof(T).Name);
            return value;
        }

        public int GetInt(string key) {
            IntPair first = null;
            foreach (var pair in Ints) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }

            if (first == null) {
                return NotifyMissing<int>(key);
            }

            return first.Value;
        }

        public float GetFloat(string key) {
            FloatPair first = null;
            foreach (var pair in Floats) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }

            if (first == null) {
                return NotifyMissing<float>(key);
            }

            return first.Value;
        }

        public string GetString(string key) {
            StringPair first = null;
            foreach (var pair in Strings) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }

            if (first == null) {
                return NotifyMissing<string>(key);
            }

            return first.Value;
        }

        public SerializedObject[] GetArray(string key) {
            ArrayPair first = null;
            foreach (var pair in Arrays) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }

            if (first == null) {
                return NotifyMissing<SerializedObject[]>(key);
            }

            return first.Value;
        }

        public SerializedObject GetObject(string key) {
            ObjectPair first = null;
            foreach (var pair in Objects) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }

            if (first == null) {
                return NotifyMissing<SerializedObject>(key);
            }

            return first.Value;
        }

        public Object GetUnityObject(string key) {
            UnityPair first = null;
            foreach (var pair in UnityObjects) {
                if (pair.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) {
                    first = pair;
                    break;
                }
            }

            if (first == null) {
                return NotifyMissing<Object>(key);
            }

            return first.Value;
        }

        public void DeserializeOnto(object token) {
            var type = token.GetType();
            foreach (var member in SerializationUtil.GetSerializedMembers(type)) {
                var fieldType = member.FieldType;
                var name = member.Name;
                var serializer = Serializers.For(fieldType);
                if (serializer == null) {
                    Debug.LogWarningFormat(
                        "[ShiroiSerialization] Couldn't find serializer for member '{0}' of type '{1}'",
                        name,
                        fieldType.FullName);
                    continue;
                }

                var value = serializer.Deserialize(name, this, fieldType);
                if (!fieldType.IsInstanceOfType(value)) {
                    if (!AllowsNull(fieldType) && value == null) {
                        continue;
                    }

                    Debug.LogWarningFormat(
                        "[ShiroiSerialization] Expected type '{1}' for member '{0}', but serializer returned ({2})!",
                        name,
                        fieldType.FullName,
                        value == null ? "null" : value.GetType().FullName
                    );
                    continue;
                }

                member.SetValue(token, value);
            }
        }

        private void Upsert<TP, T>(ICollection<TP> floats, string key, T value, Func<TP> creator) where TP : Pair<T> {
            foreach (var item in floats) {
                if (item.CompareTo(key) == 0) {
                    item.Value = value;
                }
            }

            var created = creator();
            floats.Add(created);
        }

        private static bool AllowsNull(Type type) {
            return type.IsAssignableFrom(typeof(Object));
        }

        public static SerializedObject From(object toSerialize) {
            var type = toSerialize.GetType();
            var obj = new SerializedObject();
            foreach (var member in SerializationUtil.GetSerializedMembers(type)) {
                var serializer = Serializers.For(member.FieldType);
                if (serializer == null) {
                    continue;
                }

                var value = member.GetValue(toSerialize);
                if (value == null) {
                    continue;
                }

                serializer.Serialize(value, member.Name, obj);
            }

            return obj;
        }

        public bool HasUnityObject(string key) {
            return UnityObjects.Any(pair => pair.Key == key);
        }
    }
}