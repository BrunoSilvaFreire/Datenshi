namespace Datenshi.Scripts.Entities.Systems {
    public class MainSystem : Feature {
        public MainSystem(Contexts contexts) {
            Add(new GameSystem(contexts));
        }
    }
}