using System;
using System.Collections;
using System.Collections.Generic;
using Datenshi.Scripts.UI;
using Datenshi.Scripts.UI.Misc;
using Datenshi.Scripts.Util;
using Shiroi.Cutscenes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.Cutscenes.Dialogue.UI {
    public class UIMainDialogueStage : UIDialogueStage<UIMainDialogueStage> {

        public float IndicatorYOffset;
        public Image Indicator;
        public Text CharacterLabel;
        public UIBlackBarView BlackBar;
        public UIDialoguePortrait PortraitPrefab;
        public Narrator Narrator;

        [ShowInInspector]
        private readonly List<UIDialoguePortrait> portraits = new List<UIDialoguePortrait>();

        public UIDialoguePortrait AddPortrait(Character.Character character) {
            var portrait = PortraitPrefab.Clone(transform);
            portrait.Load(character.Portrait);
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


        protected override IEnumerator DoPlayDialogue(Dialogue dialogue) {
            var speeches = dialogue.Speeches;
            foreach (var speech in speeches) {
                var character = speech.Character;
                CharacterLabel.text = character.Alias;
                var portrait = GetPortrait(character);
                if (Indicator != null) {
                    var rectTransform = (RectTransform) portrait.transform;
                    var pos = rectTransform.position;
                    pos.y = rectTransform.rect.yMax + IndicatorYOffset;
                    Indicator.rectTransform.position = pos;
                }


                foreach (var line in speech.Lines) {
                    if (line.Move) {
                        portrait.Appear(line.AppearanceMode);
                    }

                    yield return Narrator.TypeTextCharByChar(line.Text, character.SpeechClip);
                }
            }

            ClearPortraits();
        }

        private void ClearPortraits() {
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