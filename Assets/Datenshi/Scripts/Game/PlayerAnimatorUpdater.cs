using System.Collections;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities.Animation;
using Datenshi.Scripts.Graphics;
using UnityEngine;

namespace Datenshi.Scripts.Game {
    public class PlayerAnimatorUpdater : MovableEntityUpdater {
        public override void TriggerDeath() {
            GameController.Instance.RestartGame();
        }

        
    }
}