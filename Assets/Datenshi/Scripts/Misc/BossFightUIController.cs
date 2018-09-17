using System.Collections;
using Datenshi.Scripts.Util.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.Misc {
    public class BossFightUIController : Singleton<BossFightUIController> {
        public Text[] BossLabels;
        public Text[] BossShortLabels;
        public string RevealKey = "Reveal";
        public float BossBarRevealDelay = 4;
        public Animator WarningAnimator, BossBarAnimator;

        public IEnumerator InitializeUI(Character.Character boss) {
            foreach (var bossLabel in BossLabels) {
                bossLabel.text = boss.Alias;
                bossLabel.font = boss.CharacterFont;
            }
            foreach (var bossLabel in BossShortLabels) {
                bossLabel.text = boss.ShortAlias;
                bossLabel.font = boss.CharacterFont;
            }

            WarningAnimator.SetTrigger(RevealKey);
            yield return new WaitForSeconds(BossBarRevealDelay);
            BossBarAnimator.SetBool(RevealKey, true);
        }
    }
}