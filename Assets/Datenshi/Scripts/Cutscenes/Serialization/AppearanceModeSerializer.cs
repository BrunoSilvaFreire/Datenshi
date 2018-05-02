using System;
using Datenshi.Scripts.Character;
using Datenshi.Scripts.UI.Dialogue;
using Shiroi.Serialization;

namespace Datenshi.Scripts.Cutscenes.Serialization {
    public class AppearanceModeSerializer : Serializer<AppearanceMode> {
        public const string DurationKey = "Duration";
        public const string OffsetKey = "Offset";
        public const string LeftKey = "Left";

        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            var serialized = obj.GetObject(key);
            var duration = serialized.GetFloat(DurationKey);
            var offset = serialized.GetFloat(OffsetKey);
            var left = serialized.GetBoolean(LeftKey);
            return new AppearanceMode(duration, offset, left);
        }

        public override void Serialize(AppearanceMode value, string name, SerializedObject destination) {
            var obj = new SerializedObject();
            obj.SetFloat(DurationKey, value.Duration);
            obj.SetFloat(OffsetKey, value.Offset);
            obj.SetBoolean(LeftKey, value.Left);
            destination.SetObject(name, obj);
        }
    }
}