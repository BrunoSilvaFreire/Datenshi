﻿using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class CloseToCombatTarget : Conditional {
        public LivingEntity Entity;
        public SharedCombatant Target;
        public float Distance;

        public override TaskStatus OnUpdate() {
            var target = Target.Value;
            if (target == null) {
                return TaskStatus.Failure;
            }

            var d = Vector2.Distance(Entity.Center, target.Center);
            return d > Distance ? TaskStatus.Failure : TaskStatus.Success;
        }

        public override void OnDrawGizmos() {
            DebugUtil.DrawWireCircle2D(Entity.Center, Distance, Color.magenta);
        }
    }
}