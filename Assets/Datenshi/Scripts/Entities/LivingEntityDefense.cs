using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity {
        private bool defending;


        public float FocusMaxTime = 2;

        [TitleGroup(CombatGroup)]
        public float MinFocusRequired = 0.1F;

        [ShowInInspector, ReadOnly, TitleGroup(CombatGroup)]
        public bool CanFocus => FocusTimeLeft > MinFocusRequired;

        public float FocusPercent => FocusTimeLeft / FocusMaxTime;


        [ShowInInspector, ReadOnly, TitleGroup(CombatGroup)]
        public float FocusTimeLeft {
            get {
                return focusTimeLeft;
            }
            set {
                focusTimeLeft = value >= FocusMaxTime ? FocusMaxTime : value;
            }
        }

        public float DefendingFor {
            get {
                if (!defending) {
                    return 0;
                }

                return Time.time - LastDefenseStart;
            }
        }

        private bool focusingLastFrame;
        private float focusTimeLeft;
        public bool Dead => health == 0;

        public float LastDefenseStart {
            get;
            private set;
        }

        [ShowInInspector, ReadOnly, TitleGroup(CombatGroup)]
        public bool Defending {
            get {
                return defending;
            }
            set {
                if (defending == value) {
                    return;
                }

                defending = value;
                if (defending) {
                    LastDefenseStart = Time.time;
                }
            }
        }

        public void BreakDefense() { }

        private void UpdateDefense() {
            if (defending) {
                if (FocusTimeLeft <= 0) {
                    BreakDefense();
                } /* else {
                    FocusTimeLeft -= Time.deltaTime * DefenseDepleteAmountMultiplier;
                }*/
            } else {
                var recoverAmount = Time.deltaTime * FocusRecoverAmountMultiplier;
                FocusTimeLeft += recoverAmount;
            }

            if (updater == null) {
                return;
            }

            updater.SetDefending(defending);
        }
    }
}