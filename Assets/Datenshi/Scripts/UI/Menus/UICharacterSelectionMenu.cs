using Datenshi.Scripts.Character;

namespace Datenshi.Scripts.UI.Menus {
    public class UICharacterSelectionMenu : UIMenu {

        public void AddCharacter(PlayableCharacter character) {
            Instantiate(UIResources.Instance.CharacterViewPrefab, transform).Setup(character);
        }
    }
}