using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util.Buffs;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motor.States.Listeners {
    public class DashListener : MovementListener {
        public float DashSpeedMultiplier = 5;
        public float DashChangeSpeed = 0.15F;
        public float DashStaminaConsumption = 3;
        public float DashMinStaminaRequired = 4;
        private static readonly Variable<ContinuousPropertyModifier> SpeedModifier = "movement.dash.modifier";
        private static readonly Variable<LivingEntity.StaminaHandle> StaminaHandle = "movement.dash.staminaHandle";
        public static readonly Variable<bool> DashingLastFrame = "movement.dash.lastFrame";

        public override void OnTick(MovableEntity entity, StateMotor motor) {
            var lastFrame = entity.GetVariable(DashingLastFrame);
            var p = entity.InputProvider;
            var hasProvider = p != null;
            var dashDown = hasProvider && p.GetDash().Consume();
            bool dashing;
            if (!lastFrame) {
                if (dashDown) {
                    dashing = entity.CurrentStamina >= DashMinStaminaRequired;
                } else {
                    dashing = false;
                }
            } else {
                dashing = hasProvider && p.GetDashing() && entity.CurrentStamina > 0;
            }

            var m = entity.GetVariable(SpeedModifier) ?? entity.SpeedMultiplier.AddContinuousModifiers(1);

            entity.MiscController.GhostingContainer.Spawning = dashing;
            var handle = entity.GetVariable(StaminaHandle) ?? entity.GetStaminaHandle(DashStaminaConsumption);
            handle.Active = dashing;
            var target = dashing ? DashSpeedMultiplier : 1;
            m.Multiplier = Mathf.Lerp(m.Multiplier, target, DashChangeSpeed);
            entity.SetVariable(SpeedModifier, m);
            entity.SetVariable(StaminaHandle, handle);
            entity.SetVariable(DashingLastFrame, dashing);
        }
    }
}