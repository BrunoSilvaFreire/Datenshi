using System.Collections;
using System.Collections.Generic;
using Datenshi.Scripts.UI.Misc;
using Datenshi.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Dialogue {
    public class UIDialogueStage : UIMenu {
        private static UIDialogueStage instance;

        public static UIDialogueStage Instance {
            get {
                return instance ?? (instance = Load());
            }
        }

        private static UIDialogueStage Load() {
            return FindObjectOfType<UIDialogueStage>();
        }

        public float IndicatorYOffset;
        public Image Indicator;
        public UIBlackBarView BlackBar;
        public Narrator.Narrator Narrator;
        private List<UIDialoguePortrait> portraits = new List<UIDialoguePortrait>();

        public UIDialoguePortrait AddPortrait(Character.Character character) {
            var portrait = character.DialoguePortraitPrefab.Clone(transform);
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

        public void Speech(UIDialoguePortrait portrait, string text) { }

        public IEnumerator PlayDialogue(Cutscenes.Dialogue.Dialogue dialogue) {
            var speeches = dialogue.Speeches;
            foreach (var speech in speeches) {
                var character = speech.Character;
                foreach (var line in speech.Lines) {
                    var portrait = GetPortrait(character);
                    var rectTransform = ((RectTransform) portrait.transform);
                    var pos = rectTransform.position;
                    pos.y = rectTransform.rect.yMax + IndicatorYOffset;
                    Indicator.rectTransform.position = pos;
                    if (line.Move) {
                        portrait.Appear(line.AppearanceMode);
                    }
                    yield return Narrator.TypeTextCharByChar(line.Text);
                }
            }
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