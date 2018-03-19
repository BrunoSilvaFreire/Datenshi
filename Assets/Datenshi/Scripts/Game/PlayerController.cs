using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Stealth;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Game {
    public class PlayerController : MonoBehaviour {
        public PlayerInputProvider Player;

        [SerializeField, HideInInspector]
        private Entity currentEntity;

        public float SlowMoTimeScale = 0.1F;
        public float SlowMoChangeDuration = 1;
        private Tweener currentTween;
        private bool slowMo;

        [ShowInInspector]
        public Entity CurrentEntity {
            get {
                return currentEntity;
            }
            set {
                if (currentEntity != null) {
                    currentEntity.RevokeOwnership();
                }

                currentEntity = value;
                currentEntity.RequestOwnership(Player);
            }
        }

        private void Start() {
            if (currentEntity.InputProvider != Player) {
                currentEntity.RevokeOwnership();
                currentEntity.RequestOwnership(Player);
            }
        }

        private void Update() {
            var p = Player;
            if (p == null) {
                return;
            }

            var planning = p.GetPlanningMenu();
            if (slowMo != planning) {
                slowMo = planning;
                if (currentTween != null) {
                    currentTween.Kill();
                }
                var newValue = planning ? SlowMoTimeScale : 1;
                currentTween = DOTween.To(
                    () => Time.timeScale,
                    value => Time.timeScale = value,
                    newValue,
                    SlowMoChangeDuration
                );
                currentTween.onComplete = () => currentTween = null;
                currentTween.Play();
                var e = currentEntity;
                if (e == null) {
                    return;
                }
                var controller = e.AbilityController;
                if (controller == null) {
                    return;
                }
                controller.IsActive = planning;
            }
        }
    }
}