using System;
using System.Collections;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Game.Rank;
using Datenshi.Scripts.Util;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Misc {
    public class UIRankView : UICanvasGroupView {
        private PlayerController controller;
        public Text RankField;
        public Image XPBar;
        public Image XPBarBackground;
        public Animator Animator;
        public string LevelUpKey = "LevelUp";
        public string LevelDownKey = "LevelDown";
        public float RankXPGainedTransitionDuration;
        public Text DamageLabel;
        public Gradient ColorGradient;
        public float BackgroundBrightnessLoss = 0.25F;
        private bool tracking;

        private void Start() {
            controller = PlayerController.Instance;
            controller.PlayerRankXPGainedEvent.AddListener(OnXPGained);
            UpdateText(controller.Rank);
            tracking = true;
        }

        private void UpdateText(Rank r) {
            if (DamageLabel != null) {
                var damage = GameResources.Instance.RankDamageGraph.Evaluate((byte) r.CurrentLevel);
                DamageLabel.text = $"Damage {damage * 100}%";
            }
        }

        private Coroutine xpGainedRoutine;

        private void OnXPGained(float xpGained) {
            CoroutineUtil.ReplaceCoroutine(ref xpGainedRoutine, this, DoXPEffect(xpGained));
        }

        private IEnumerator DoXPEffect(float xpGained) {
            tracking = false;
            var r = controller.Rank;
            XPBar.DOFillAmount(r.RankPercentage, RankXPGainedTransitionDuration);


            yield return new WaitForSeconds(RankXPGainedTransitionDuration);
            var left = controller.RankXPGainedWaitDuration - RankXPGainedTransitionDuration;
            if (left > 0) {
                yield return new WaitForSeconds(left);
            }

            tracking = true;
        }

        private void SetColor(Rank rank) {
            var position = ((float) rank.CurrentLevel + rank.RankPercentage) / Rank.MaxRankLevel;
            var color = ColorGradient.Evaluate(position);
            DamageLabel.color = color;
            XPBar.color = color;
            XPBarBackground.color = color.SetBrightness(color.GetBrightness() - BackgroundBrightnessLoss);
        }

        private RankLevel lastRank = RankLevel.F;

        private void Update() {
            var r = controller.Rank;
            XPBar.fillAmount = r.RankPercentage;
            var currentRank = r.CurrentLevel;
            SetColor(r);
            if (lastRank == currentRank) {
                return;
            }

            RankField.text = Enum.GetName(typeof(RankLevel), controller.Rank.CurrentLevel);
            UpdateText(r);

            if (lastRank > currentRank) {
                Animator.SetTrigger(LevelDownKey);
            }

            if (lastRank < currentRank) {
                Animator.SetTrigger(LevelUpKey);
            }

            lastRank = currentRank;
        }
    }
}