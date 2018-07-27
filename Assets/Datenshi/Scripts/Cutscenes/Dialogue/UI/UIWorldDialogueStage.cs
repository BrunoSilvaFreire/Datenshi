using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Dialogue.UI {
    public class UIWorldDialogueStage : UIDialogueStage<UIWorldDialogueStage> {
        protected override IEnumerator DoPlayDialogue(Dialogue dialogue) {
            var speeches = dialogue.Speeches;
            var con = new HashSet<EntityMiscController>();
            foreach (var speech in speeches) {
                var character = speech.Character;
                var entity = FindEntityForCharacter(character);
                foreach (var line in speech.Lines) {
                    var c = entity.MiscController;
                    c.ShowCanvas();
                    con.Add(c);
                    var narrator = c.EntityNarrator;
                    yield return narrator.TypeTextCharByChar(line.Text, character.SpeechClip);
                }
            }

            foreach (var entityMiscController in con) {
                entityMiscController.HideCanvas();
            }
        }

        private readonly Dictionary<Character.Character, Entity> entityCache =
            new Dictionary<Character.Character, Entity>();

        private Entity FindEntityForCharacter(Character.Character character) {
            if (entityCache.ContainsKey(character)) {
                return entityCache[character];
            }

            return entityCache[character] = FindEntityInScene(character);
        }

        private static Entity FindEntityInScene(Character.Character character) {
            return ObjectUtil.FindAll<Entity>().FirstOrDefault(entity => entity.Character == character);
        }
    }
}