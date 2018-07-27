using Datenshi.Scripts.World.Rooms.Game;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Editor {
    [CustomEditor(typeof(WorldTimeProgresser))]
    public class WorldTimeProgresserEditor : UnityEditor.Editor {
        private WorldTimeProgresser progresser;

        private void OnEnable() {
            progresser = (WorldTimeProgresser) target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            WorldTimeProgresser.currentPreview = GUILayout.HorizontalSlider(WorldTimeProgresser.currentPreview, 0, 1);
            DoPreview();
        }

        private void DoPreview() {
            WorldTimeProgresser.UpdateLight(WorldTimeProgresser.currentPreview,
                progresser.SkyboxMaterial, progresser.SkyColor, progresser.ColorGradient, progresser.Intensity,
                progresser.AmbientIntensity, progresser.AtmosphereTickness);
        }
    }
}