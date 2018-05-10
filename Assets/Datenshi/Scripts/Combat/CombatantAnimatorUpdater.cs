using Datenshi.Scripts.Animation;

namespace Datenshi.Scripts.Combat {
    public abstract class CombatantAnimatorUpdater : AnimatorUpdater {
        public abstract void TriggerAttack();
        public abstract void TriggerAttack(string attack);
        public abstract void SetDefend(bool defend);
        public abstract void SetTrigger(string key);

        public abstract void TriggerDeflect();

        public abstract void TriggerCounter();

        public abstract void TriggerDeath();
        public abstract void TriggerSpawn();
    }
}