using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Motors;
using Datenshi.Scripts.Entities.Motors.State.Ground;
using UnityEngine;

namespace Datenshi.Scripts.UI.Misc {
    public class UIDashElement : UIMaxedCharacterBarElement {
        protected override float GetPercentage(Entity entity) {
            var config = entity.Config as GroundMotorConfig;
            if (config == null) {
                return 0;
            }

            var totalDash = Time.time - entity.GetVariable(DashGroundMotorState.DashStart);
            return totalDash / config.DashCooldown;
        }
    }
}