using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Interaction {
    public interface IDefendable {
        bool CanDefend(LivingEntity entity);
        void Defend(LivingEntity entity);
        bool CanPoorlyDefend(LivingEntity entity);
        void PoorlyDefend(LivingEntity entity);
        DefenseType GetDefenseType();
    }

    public enum DefenseType {
        Deflect,
        Counter
    }
}