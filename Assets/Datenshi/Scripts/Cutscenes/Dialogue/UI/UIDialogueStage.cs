using System.Collections;
using System.Collections.Generic;
using Datenshi.Scripts.UI;
using Datenshi.Scripts.UI.Misc;
using Shiroi.Cutscenes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.Cutscenes.Dialogue.UI {
    public class UIDialogueStage : UIMenu {
        private static UIDialogueStage instance;

        public static UIDialogueStage Instance => instance ? instance : (instance = Load());

        private static UIDialogueStage Load() {
            return FindObjectOfType<UIDialogueStage>();
        }

        public float IndicatorYOffset;
        public Image Indicator;
        public Text CharacterLabel;
        public UIBlackBarView BlackBar;
        public Scripts.UI.Narrator.Narrator Narrator;

        [ShowInInspector]
        private readonly List<UIDialoguePortrait> portraits = new List<UIDialoguePortrait>();

        public UIDialoguePortrait AddPortrait(Character.Character character) {
            //todo fix
            //character.Portrait.Clone(transform)
            var portrait = (UIDialoguePortrait) null;
            var rect = (RectTransform) portrait.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.right;
            portrait.SnapShowing(false);
            portraits.Add(portrait);
            return portrait;
        }

        public UIDialoguePortrait AddPortrait(Character.Character character, AppearanceMode mode) {
            var portrait = AddPortrait(character);
            portrait.Appear(mode);
            return portrait;
        }

        protected override void SnapShow() {
            BlackBar.SnapShowing(true);
        }

        protected override void SnapHide() {
            BlackBar.SnapShowing(false);
        }

        protected override void OnShow() {
            BlackBar.Showing = true;
            foreach (var portrait in portraits) {
                portrait.Showing = true;
            }
        }

        protected override void OnHide() {
            BlackBar.Showing = false;
            foreach (var portrait in portraits) {
                portrait.Showing = false;
            }
        }


        public IEnumerator PlayDialogue(Cutscenes.Dialogue.Dialogue dialogue) {
            var speeches = dialogue.Speeches;
            Debug.Log("Playing dialogue " + dialogue);
            foreach (var speech in speeches) {
                var character = speech.Character;
                Debug.Log("Playing speech " + speech + " for " + character);
                CharacterLabel.text = character.Alias;
                foreach (var line in speech.Lines) {
                    Debug.Log("Playing line " + line);

                    var portrait = GetPortrait(character);
                    var rectTransform = (RectTransform) portrait.transform;
                    if (Indicator != null) {
                        var pos = rectTransform.position;
                        pos.y = rectTransform.rect.yMax + IndicatorYOffset;
                        Indicator.rectTransform.position = pos;
                    }

                    if (line.Move) {
                        portrait.Appear(line.AppearanceMode);
                    }

                    yield return Narrator.TypeTextCharByChar(line.Text);
                }
            }

            foreach (var portrait in portraits) {
                Destroy(portrait.gameObject);
            }

            portraits.Clear();
        }


        private UIDialoguePortrait GetPortrait(Character.Character character) {
            foreach (var portrait in portraits) {
                if (portrait.Character == character) {
                    return portrait;
                }
            }

            return AddPortrait(character);
        }
    }
}