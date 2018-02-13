namespace Datenshi.Scripts.Entities.Systems.Movement {
    public class MovementSystem : Feature {
        public MovementSystem(Contexts contexts) {
            Add(new GroundMovementSystem(contexts));
        }
    }
}