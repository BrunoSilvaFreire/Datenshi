using Datenshi.Scripts.Controller;
using Entitas;

namespace Datenshi.Scripts.Entities.Components.Input {
    public class ControllableComponent : IComponent {
        public IInputProvider Provider;
    }
}