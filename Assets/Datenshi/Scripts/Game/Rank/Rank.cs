using System;
using Datenshi.Scripts.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Game.Rank {
    public enum RankLevel : byte {
        F,
        E,
        D,
        C,
        B,
        A,
        S,
        SS,
        SSS
    }

    [Serializable]
    public class Rank {
        public const byte ExtraDamagePerLevel = 5;
        private float xp;

        public float RankPercentage => XP / NextLevelRequiredXP;

        public float NextLevelRequiredXP => GameResources.Instance.RankRequiredXPGraph.Evaluate((byte) CurrentLevel);

        [ShowInInspector]
        public float XP {
            get {
                return xp;
            }
            set {
                var requiredXP = NextLevelRequiredXP;
                if (CurrentLevel > RankLevel.F && value < 0) {
                    var required = GameResources.Instance.GetRequiredXP((byte) (CurrentLevel - 1));
                    CurrentLevel--;
                    XP = required + value;
                    return;
                }

                if (value >= requiredXP) {
                    LevelUp((int) (value / requiredXP));
                    xp = value % requiredXP;
                    return;
                }

                xp = value;
            }
        }

        [ShowInInspector, ReadOnly]
        public RankLevel CurrentLevel {
            get;
            private set;
        }

        public float GetDamageMultiplier() {
            return GameResources.Instance.RankDamageGraph.Evaluate((byte) CurrentLevel);
        }

        public void LevelUp(int levels = 1, bool resetXP = true) {
            if (CurrentLevel == RankLevel.SSS) {
                return;
            }

            CurrentLevel = (RankLevel) ((int) CurrentLevel + levels);
            if (resetXP) {
                xp = 0;
            }
        }
    }
}