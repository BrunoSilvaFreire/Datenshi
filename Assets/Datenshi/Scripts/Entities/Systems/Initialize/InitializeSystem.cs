namespace Datenshi.Scripts.Entities.Systems.Initialize {
    public class InitializeSystem : Feature {
        public InitializeSystem(Contexts contexts, params int[] players) {
            Add(new InitializeViewSystem(contexts));
            Add(new InitializeGroundMovementSystem(contexts));
            Add(new InitializePlayerSystem(contexts, players));
        }
    }
}