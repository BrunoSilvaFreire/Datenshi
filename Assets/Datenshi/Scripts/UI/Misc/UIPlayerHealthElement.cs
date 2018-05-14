﻿using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.UI.Misc {
    public class UIPlayerHealthElement : UIHealthElement {
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

            UpdateBar();
        }


        private void OnDamaged() {
            UpdateBar();
        }

        protected override LivingEntity GetEntity() {
            return Player.CurrentEntity as LivingEntity;
        }
    }
}