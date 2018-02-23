using Datenshi.Scripts.Entities.Systems.Player;

namespace Datenshi.Scripts.Entities.Systems {
    public class GameSystem : Feature {
        public GameSystem(Contexts contexts) {
            Add(new VelocityExecutionSystem(contexts));
            Add(new ExecutePlayerInputSystem(contexts));
        }
    }
}