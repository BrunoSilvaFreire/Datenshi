using Entitas;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Systems.Initialize {
    public class InitializeUISystem : IInitializeSystem {
        private readonly UIContext context;

        public InitializeUISystem(Contexts c) {
            context = c.uI;
        }

        public void Initialize() {
            var canvas = new GameObject("MainCanvas").AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 0.5F;
            context.SetMainCanvas(canvas);
        }
    }
}