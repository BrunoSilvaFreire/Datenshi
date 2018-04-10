using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Interaction {
    public abstract class Defendable : MonoBehaviour {
        public abstract bool CanDefend(LivingEntity entity);
        public abstract void Defend(LivingEntity entity);
        public abstract bool CanPoorlyDefend(LivingEntity entity);
        public abstract void PoorlyDefend(LivingEntity entity);
        public abstract DefenseType GetDefenseType();
    }

    public enum DefenseType {
        Deflect,
        Counter
    }
}