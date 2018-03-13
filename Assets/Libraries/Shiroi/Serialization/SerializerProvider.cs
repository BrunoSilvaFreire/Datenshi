using System.Collections.Generic;
using UnityEngine;

namespace Shiroi.Serialization {
    public abstract class SerializerProvider : ScriptableObject {
        public abstract IEnumerable<Serializer> Provide();
    }
}