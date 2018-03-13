using System;

namespace Shiroi.Serialization {
    public abstract class RuntimeSerializerProvider {
        public abstract bool Supports(Type type);
        public abstract Serializer Provide(Type type);
    }
}