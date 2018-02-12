using Datenshi.Scripts.Entities.Systems.Initialize;

namespace Datenshi.Scripts.Entities.Systems {
    public class GameSystem : Feature {
        public GameSystem(Contexts contexts) {
            Add(new InitializeViewSystem(contexts));
            Add(new VelocityExecutionSystem(contexts));
        }
    }
}