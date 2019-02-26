using System.Collections;
using System.Collections.Generic;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Entities.Misc.Narrator;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.UI.Misc;
using Datenshi.Scripts.Util;
using DG.Tweening;
using Shiroi.Cutscenes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Datenshi.Scripts.Cutscenes.Dialogue.UI {
    public class UIMainDialogueStage : UIDialogueStage<UIMainDialogueStage> {
        public float IndicatorYOffset;
        public Image Indicator;
        public Text CharacterLabel;
        public UIBlackBarView BlackBar;
        public UIDialoguePortrait PortraitPrefab;
        public Narrator Narrator;
        public Text ContinueInstruction;
        public Text SkipInstruction;
        public float InstructionFadeDuration;
        public UICircle SkipProgressCircle;
        public float SkipRequiredCircle = 2;

        public void SetShowContinueInstruction(bool show) {
            ContinueInstruction.DOKill();
            ContinueInstruction.DOFade(show ? 1 : 0, InstructionFadeDuration);
        }

        public void SetShowSkipInstruction(bool show) {
            SkipInstruction.DOKill();
            SkipInstruction.DOFade(show ? 1 : 0, InstructionFadeDuration);
        }

        [ShowInInspector]
        private readonly List<UIDialoguePortrait> portraits = new List<UIDialoguePortrait>();

        public float SkipCompleteIndicationDuration = 1;
        public float SkipCompleteIndicationScale = 2;
        private float lastStart;
        private bool reelegible = true;

        private void Awake() {
            foreach (var player in FindObjectsOfType<CutscenePlayer>()) {
                Debug.Log($"Registering player {player}");
                player.OnPlay.AddListener(OnCutscenePlay);
                player.OnPlayed.AddListener(OnCutscenePlayed);
            }
        }

        private readonly List<CutsceneExecutor> activeExecutors = new List<CutsceneExecutor>();

        private void OnCutscenePlayed(CutsceneExecutor arg0) {
            activeExecutors.Remove(arg0);
        }

        private void OnCutscenePlay(CutsceneExecutor arg0) {
            activeExecutors.Add(arg0);
        }

        private void Update() {
            if (!Showing) {
                return;
            }

            var p = PlayerController.Instance.Player.CurrentPlayer;
            if (!reelegible) {
                reelegible = !p.GetButton((int) Actions.Submit);
                return;
            }

            if (p.GetButtonDown((int) Actions.Submit)) {
                lastStart = Time.time;
                return;
            }

            if (!p.GetButton((int) Actions.Submit)) {
                SkipProgressCircle.SetArc(0);
                return;
            }

            var totalTime = Time.time - lastStart;
            if (totalTime >= SkipRequiredCircle) {
                Complete();
                return;
            }

            SkipProgressCircle.SetArc(totalTime / SkipRequiredCircle);
        }

        public UnityEvent OnSkip;

        private void Complete() {
            reelegible = false;
            foreach (var executor in activeExecutors) {
                Debug.Log("Skipped");
                executor.Skip();
            }

            OnSkip.Invoke();
            SkipProgressCircle.Arc = 1;
            var t = SkipProgressCircle.rectTransform;
            t.DOScale(SkipCompleteIndicationScale, SkipCompleteIndicationDuration)
                .OnComplete(
                    () => {
                        t.localScale = Vector3.one;
                        SkipProgressCircle.SetArc(0);
                    });
            SkipProgressCircle.DOFade(0, SkipCompleteIndicationDuration)
                .OnComplete(
                    () => SkipProgressCircle.SetAlpha(1)
                );
        }

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
            CharacterLabel.SetAlpha(1);
            Narrator.textComponent.SetAlpha(1);
            ContinueInstruction.SetAlpha(1);
            SkipInstruction.SetAlpha(1);
            BlackBar.SnapShowing(true);
        }

        protected override void SnapHide() {
            CharacterLabel.SetAlpha(0);
            Narrator.textComponent.SetAlpha(0);
            ContinueInstruction.SetAlpha(0);
            SkipInstruction.SetAlpha(0);
            BlackBar.SnapShowing(false);
        }

        protected override void OnShow() {
            BlackBar.Showing = true;
            SetGraphicFade(CharacterLabel, 1);
            SetGraphicFade(Narrator.textComponent, 1);
            SetGraphicFade(ContinueInstruction, 1);
            SetGraphicFade(SkipInstruction, 1);
            foreach (var portrait in portraits) {
                portrait.Showing = true;
            }
        }

        private void SetGraphicFade(Graphic g, float value) {
            g.DOKill();
            g.DOFade(value, GroupTransitionDuration);
        }

        protected override void OnHide() {
            BlackBar.Showing = false;
            SetGraphicFade(CharacterLabel, 0);
            SetGraphicFade(Narrator.textComponent, 0);
            SetGraphicFade(ContinueInstruction, 0);
            SetGraphicFade(SkipInstruction, 0);
            foreach (var portrait in portraits) {
                portrait.Showing = false;
            }
        }


        protected override IEnumerator DoPlayDialogue(Dialogue dialogue) {
            var speeches = dialogue.Speeches;
            BlackBar.ShowDialogue = true;
            ResetInstructions();
            SetShowInstructions(true);
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
            BlackBar.ShowDialogue = false;
        }

        private void SetShowInstructions(bool b) {
            SetShowContinueInstruction(b);
            SetShowSkipInstruction(b);
        }

        private void ResetInstructions() {
            ContinueInstruction.DOKill();
            ContinueInstruction.SetAlpha(0);
            SkipInstruction.DOKill();
            ContinueInstruction.SetAlpha(0);
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