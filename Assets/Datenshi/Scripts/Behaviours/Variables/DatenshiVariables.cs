using System;
using BehaviorDesigner.Runtime;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Variables {
    [Serializable]
    public class SharedCombatant : SharedVariable<ICombatant> { }

    [Serializable]
    public class SharedLivingEntity : SharedVariable<LivingEntity> { }
}