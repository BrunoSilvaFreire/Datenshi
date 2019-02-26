using System;
using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Datenshi.Scripts.Master.Behaviour {
    [Serializable]
    public class GetCurrentPlayerEntity : Action {
        public SharedEntity EntityResult;
        public SharedLivingEntity LivingResult;
        public SharedMovableEntity MovableResult;

        public override TaskStatus OnUpdate() {
            var r = PlayerController.Instance.CurrentEntity;
            EntityResult.Value = r;
            LivingResult.Value = r as LivingEntity;
            MovableResult.Value = r as MovableEntity;
            return TaskStatus.Success;
        }
    }
}