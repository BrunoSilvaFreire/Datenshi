using System;
using BehaviorDesigner.Runtime;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;

namespace Datenshi.Scripts.Behaviours.Variables {
    [Serializable]
    public class SharedCombatant : SharedVariable<ICombatant> { }

    [Serializable]
    public class SharedEntity : SharedVariable<Entity> { }

    [Serializable]
    public class SharedLivingEntity : SharedVariable<LivingEntity> { }

    [Serializable]
    public class SharedMovableEntity : SharedVariable<MovableEntity> { }
}