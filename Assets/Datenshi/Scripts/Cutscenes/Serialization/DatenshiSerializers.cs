using Shiroi.Serialization;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Serialization {

    public static class DatenshiSerializers {
        [RuntimeInitializeOnLoadMethod]
        static void Init() {
            Serializers.RegisterSerializer(new AppearanceModeSerializer());
        }
    }
}