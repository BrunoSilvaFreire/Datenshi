using System.Linq;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Entities;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using Shiroi.FX.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class DummyUpdater : CombatantAnimatorUpdater {
        public ActiveSkill[] Attacks;

        [Required]
        public LivingEntity Entity;

        public Effect DeathEffect;
        public Effect SpawnEffect;

        public override void TriggerAttack(string attack = DefaultAttackName) {
            var found = Attacks.FirstOrDefault(a => a.name == attack);
            if (found != null) {
                found.Execute(Entity);
            } else {
                Debug.LogWarning("Found no attack named '" + attack + "'", this);
            }
        }

        protected override void UpdateAnimator(Animator anim) { }

        public override void SetDefending(bool defend) { }

        public override void SetTrigger(string key) { }

        public override void TriggerDeath() {
            if (DeathEffect != null) {
                DeathEffect.PlayIfPresent(this);
            }

            Destroy(transform.parent.gameObject);
        }

        public override void TriggerSpawn() {
            if (SpawnEffect != null) {
                SpawnEffect.PlayIfPresent(this);
            }
        }

        public override void SetBool(string key, bool value) { }

        public override void TriggerDefend() { }
    }
}