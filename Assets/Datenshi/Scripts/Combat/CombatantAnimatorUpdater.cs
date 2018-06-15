using Datenshi.Scripts.Animation;
using UnityEngine;

namespace Datenshi.Scripts.Combat {
    public abstract class CombatantAnimatorUpdater : AnimatorUpdater {
        public const string DefaultAttackName = "Attack";
        public abstract void TriggerAttack(string attack = DefaultAttackName);
        public abstract void TriggerDeath();
        public abstract void TriggerSpawn();
        //public abstract void TriggerAgressiveDefend();
        //public abstract void TriggerEvasiveDefend();
        public abstract void SetDefending(bool defend);
        public abstract void SetTrigger(string key);
        public abstract void SetBool(string key, bool p1);


        public void SetAnimationTimeIndependent(bool independent) {
            Animator.updateMode = independent ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
        }
    }
}