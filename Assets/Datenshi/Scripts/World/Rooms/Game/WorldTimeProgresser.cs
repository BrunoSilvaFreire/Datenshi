using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Game {
    [ExecuteInEditMode]
    public class WorldTimeProgresser : AbstractRoomMember {
        public float Start;
        public float End;
        public Gradient ColorGradient;
        public AnimationCurve Intensity;
        public AnimationCurve AtmosphereTickness;
        public Material SkyboxMaterial;
        public Gradient SkyColor;
        public AnimationCurve AmbientIntensity;
        private float currentPosition;
        private float bestPosition;

        public float Distance => End - Start;

        private void Update() {
            var instance = PlayerController.Instance;
            if (instance == null) {
                return;
            }

            var entity = instance.CurrentEntity;
            if (entity == null) {
                return;
            }

            var entX = entity.Center.x;
            currentPosition = Mathf.Clamp01((entX - Start) / (End - Start));
            UpdateLight();
        }

        private void UpdateLight() {
            if (currentPosition > bestPosition) {
                bestPosition = currentPosition;
            } else {
                return;
            }

            UpdateLight(bestPosition, SkyboxMaterial, SkyColor, ColorGradient, Intensity, AmbientIntensity,
                AtmosphereTickness);
        }

        public static void UpdateLight(float position, Material skyboxMaterial, Gradient skyboxColor,
            Gradient colorGradient, AnimationCurve intensity, AnimationCurve ambientIntensity,
            AnimationCurve atmosphereTickness) {
            var l = World.Instance.SunLight;
            l.color = colorGradient.Evaluate(position);
            l.intensity = intensity.Evaluate(position);
            var color = skyboxColor.Evaluate(position);
            skyboxMaterial.SetColor("_SkyTint", color);
            skyboxMaterial.SetFloat("_AtmosphereThickness", atmosphereTickness.Evaluate(position));
            RenderSettings.ambientIntensity = ambientIntensity.Evaluate(position);
            DynamicGI.UpdateEnvironment();
        }

#if UNITY_EDITOR
        public const float MetersPerLines = 2;
        public const float Zet = -20;
        public static float currentPreview;
        private void OnDrawGizmos() {
            var distance = Distance;
            var totalLines = (int) (distance / MetersPerLines);
            for (var i = 0; i < totalLines - 1; i++) {
                var pos = Start + i * MetersPerLines;
                var posB = Start + (i + 1) * MetersPerLines;
                var percent = (float) i / totalLines;
                Gizmos.color = ColorGradient.Evaluate(percent);
                Gizmos.DrawLine(new Vector3(pos, 0, Zet), new Vector3(posB, 0, Zet));
            }
        }
#endif
    }
}