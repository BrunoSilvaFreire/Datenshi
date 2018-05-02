using Datenshi.Scripts.Entities;
using UnityEngine;
using UPM.Motors.Config;

namespace Datenshi.Scripts.UI.Misc {
    public class UIDashElement : UIMaxedCharacterBarElement {
        protected override float GetPercentage(Entity entity) {
            var l = entity as MovableEntity;
            if (l == null) {
                return 0;
            }

            var config = l.MotorConfig as GroundMotorConfig;
            if (config == null) {
                return 0;
            }

            //var totalDash = Time.time - entity.GetVariable(DashGroundMotorState.DashStart);
            //return totalDash / config.DashCooldown;
            return 0;
        }
    }
}