using Sirenix.OdinInspector;
using UnityEngine;
using UPM.Util;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity {
        private bool defending;

        public float DefendTimePercent => DefendTimeLeft / FocusMaxTime;

        [SerializeField]
        private Bounds2D defenseHitbox;

        public float FocusMaxTime = 2;

        [TitleGroup(CombatGroup)]
        public float MinDefenseRequired = 0.1F;

        [TitleGroup(CombatGroup)]
        public float DefenseRecoverAmountMultiplier = 1;

        [TitleGroup(CombatGroup)]
        public float DefenseDepleteAmountMultiplier = 2;

        [ShowInInspector, ReadOnly, TitleGroup(CombatGroup)]
        public bool CanDefend => DefendTimeLeft > MinDefenseRequired;

        public Bounds2D DefenseHitbox {
            get {
                var hb = defenseHitbox;
                hb.Center.x *= CurrentDirection.X;
                hb.Center += (Vector2) transform.position;
                return hb;
            }
        }

        [ShowInInspector, ReadOnly, TitleGroup(CombatGroup)]
        public float DefendTimeLeft {
            get;
            set;
        }

        public float DefendingFor {
            get {
                if (!defending) {
                    return 0;
                }

                return Time.time - LastDefenseStart;
            }
        }

        private bool canReDefend;
        private bool defendingLastFrame;
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
                if (!defending && value && !canReDefend) {
                    return;
                }

                if (defending == value) {
                    return;
                }

                if (value && !CanDefend) {
                    return;
                }

                defending = value;
                if (defending) {
                    LastDefenseStart = Time.time;
                }
            }
        }

        private void UpdateFocus() {
            if (defending) {
                if (DefendTimeLeft <= 0) {
                    defending = false;
                } else {
                    DefendTimeLeft -= Time.deltaTime * DefenseDepleteAmountMultiplier;
                }
            } else {
                var recoverAmount = Time.deltaTime * DefenseRecoverAmountMultiplier;
                if (DefendTimeLeft + recoverAmount > FocusMaxTime) {
                    DefendTimeLeft = FocusMaxTime;
                } else {
                    DefendTimeLeft += recoverAmount;
                }
            }

            var p = InputProvider;
            if (p != null) {
                var pressingDefend = p.GetDefend();
                if (!Defending) {
                    if (!canReDefend) {
                        canReDefend = !pressingDefend;
                    }
                } else if (defendingLastFrame && !CanDefend) {
                    canReDefend = false;
                }
            }

            defendingLastFrame = Defending;

            if (updater == null) {
                return;
            }

            updater.SetDefending(defending);
        }
    }
}