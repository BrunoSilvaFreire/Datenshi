namespace Datenshi.Scripts.Entities.Systems.Initialize {
    public class InitializeSystem : Feature {
        public InitializeSystem(Contexts contexts) {
            Add(new InitializeViewSystem(contexts));
            Add(new InitializeGroundMovementSystem(contexts));
        }
    }
}