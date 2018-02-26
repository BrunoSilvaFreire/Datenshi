using System;
using Sirenix.Serialization;

namespace Datenshi.Scripts.Entities.Blueprints {
    [Serializable]
    public class SerializableMember {
        public string name;

        [NonSerialized, OdinSerialize]
        public object value;

        public SerializableMember() { }

        public SerializableMember(string name, object value) {
            this.name = name;
            this.value = value;
        }
    }
}