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
    public class UIRankElement : UICanvasGroupElement {
        private PlayerController controller;
        public Text RankField;
        public Image XPBar;
        public Animator Animator;
        public string LevelUpKey = "LevelUp";
        public string LevelDownKey = "LevelDown";
        public float RankXPGainedTransitionDuration;
        public float DamageFadeDuration;
        public float DamageResizeDuration;
        public Text DamageLabel;
        public RectTransform DamageTransform;
        private bool tracking;

        private void Start() {
            controller = PlayerController.Instance;
            controller.PlayerRankXPGainedEvent.AddListener(OnXPGained);
            tracking = true;
        }

        private Coroutine xpGainedRoutine;

        private void OnXPGained(float xpGained) {
            CoroutineUtil.ReplaceCoroutine(ref xpGainedRoutine, this, DoXPEffect(xpGained));
        }

        private IEnumerator DoXPEffect(float xpGained) {
            tracking = false;
            var r = controller.Rank;
            XPBar.DOFillAmount(r.RankPercentage, RankXPGainedTransitionDuration);
            DamageLabel.SetAlpha(0);
            var damage = GameResources.Instance.RankDamageGraph.Evaluate((byte) r.CurrentLevel);
            DamageLabel.text = $"Damage +{damage}%";
            yield return new WaitForSeconds(RankXPGainedTransitionDuration);
            var left = controller.RankXPGainedWaitDuration - RankXPGainedTransitionDuration;
            if (left > 0) {
                yield return new WaitForSeconds(left);
            }

            tracking = true;
        }

        private RankLevel lastRank = RankLevel.F;

        private void Update() {
            var r = controller.Rank;
            XPBar.fillAmount = r.RankPercentage;
            var currentRank = r.CurrentLevel;
            if (lastRank == currentRank) {
                return;
            }

            RankField.text = Enum.GetName(typeof(RankLevel), controller.Rank.CurrentLevel);

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