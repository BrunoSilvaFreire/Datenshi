using System;
using Shiroi.Serialization;

namespace Shiroi.Cutscenes.Serialization {
    public class AppearanceModeSerializer : Serializer<AppearanceMode> {
        public const string DurationKey = "Duration";
        public const string LeftKey = "Left";
        public const string OffsetKey = "Offset";

        public override object Deserialize(string key, SerializedObject obj, Type fieldType) {
            var ser = obj.GetObject(key);
            var duration = ser.GetFloat(DurationKey);
            var offset = ser.GetFloat(OffsetKey);
            var left = ser.GetBoolean(LeftKey);
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