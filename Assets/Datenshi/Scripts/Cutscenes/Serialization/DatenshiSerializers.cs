using Shiroi.Serialization;
using UnityEditor;

namespace Datenshi.Scripts.Cutscenes.Serialization {
    [InitializeOnLoad]
    public static class DatenshiSerializers {
        static DatenshiSerializers() {
            Serializers.RegisterSerializer(new AppearanceModeSerializer());
        }
    }
}