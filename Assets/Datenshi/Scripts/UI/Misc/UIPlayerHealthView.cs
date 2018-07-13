using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.UI.Misc {
    public class UIPlayerHealthView : UIHealthView {
        public PlayerController Player;

        private void Awake() {
            Player.OnEntityChanged.AddListener(OnChanged);
        }


        private void OnChanged(Entity oldEntity, Entity newEntity) {
            var oldLiving = oldEntity as LivingEntity;
            if (oldLiving != null) {
                oldLiving.OnHealthChanged.RemoveListener(OnDamaged);
            }

            var newLiving = newEntity as LivingEntity;
            if (newLiving != null) {
                newLiving.OnHealthChanged.AddListener(OnDamaged);
            }

            Debug.Log("Changed");
            UpdateBar();
        }


        private void OnDamaged() {
            Debug.Log("Updating");
            UpdateBar();
        }

        protected override LivingEntity GetEntity() {
            Debug.Log("Entity = "+Player.CurrentEntity);
            return Player.CurrentEntity as LivingEntity;
        }
    }
}