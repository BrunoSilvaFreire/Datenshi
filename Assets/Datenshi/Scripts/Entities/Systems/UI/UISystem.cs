namespace Datenshi.Scripts.Entities.Systems.UI {
    public class UISystem : Feature {
        public UISystem(Contexts contexts) {
            Add(new CharacterSelectionAddEntitySystem(contexts));
        }
    }
}