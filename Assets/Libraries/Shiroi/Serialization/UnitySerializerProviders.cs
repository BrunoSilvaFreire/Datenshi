using System;
using Shiroi.Serialization.Util;

namespace Shiroi.Serialization {
    public class GenericRuntimeSerializerProvider : RuntimeSerializerProvider {
        private readonly Type supportedType;
        private readonly Type serializerType;

        public GenericRuntimeSerializerProvider(Type supportedType, Type serializerType) {
            this.supportedType = supportedType;
            this.serializerType = serializerType;
        }

        public override bool Supports(Type type) {
            return type.IsGenericType && TypeUtil.IsInstanceOfGenericType(supportedType, type);
        }

        public override Serializer Provide(Type type) {
            var genericType = type.GetGenericArguments()[0];
            return (Serializer) Activator.CreateInstance(serializerType.MakeGenericType(genericType));
        }
    }
}