using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Components.UI {
    [Unique, UI]
    public class MainCanvasComponent : IComponent {
        public Canvas Canvas;
    }
}