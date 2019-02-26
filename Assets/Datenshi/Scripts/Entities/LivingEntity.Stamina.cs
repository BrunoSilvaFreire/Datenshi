using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Util;
using Lunari.Tsuki;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity {
        public const string StaminaGroup = "Stamina";

        [ShowInInspector, BoxGroup(StaminaGroup)]
        public float CurrentStamina {
            get {
                return currentStamina;
            }
            set {
                currentStamina = Mathf.Clamp(value, 0, MaxStamina);
            }
        }

        [BoxGroup(StaminaGroup)]
        public float MaxStamina = 20;

        [BoxGroup(StaminaGroup)]
        public float StaminaRegain = 4;

        [ShowInInspector, BoxGroup(StaminaGroup)]
        public float StaminaPercentage => currentStamina / MaxStamina;

        public class StaminaHandle {
            private readonly LivingEntity entity;

            [ShowInInspector]
            public bool Active {
                get;
                set;
            }

            public UnityAction OnEmptyCallback {
                get;
            }

            internal StaminaHandle(LivingEntity entity, float consumption, UnityAction onEmpty) {
                this.entity = entity;
                this.Consumption = consumption;
                OnEmptyCallback = onEmpty;
            }

            [ShowInInspector]
            public float Consumption {
                get;
            }

            public void Cancel() {
                entity.staminaUsage.Remove(this);
            }
        }

        [ShowInInspector, BoxGroup(StaminaGroup), ReadOnly]
        private readonly List<StaminaHandle> staminaUsage = new List<StaminaHandle>();

        [SerializeField, HideInInspector]
        private float currentStamina;

        public bool IsStaminaBeingUsed => !staminaUsage.IsEmpty();

        public StaminaHandle GetStaminaHandle(float consumption, UnityAction onEmpty = null) {
            var handle = new StaminaHandle(this, consumption, onEmpty);
            staminaUsage.Add(handle);
            return handle;
        }

        private void UpdateStamina() {
            var handle = staminaUsage.FirstOrDefault(staminaHandle => staminaHandle.Active);
            if (handle == null) {
                CurrentStamina += StaminaRegain * Time.deltaTime;
            } else {
                var c = handle.Consumption * Time.deltaTime;
                if (CurrentStamina > c) {
                    CurrentStamina -= c;
                } else {
                    CurrentStamina = 0;
                    handle.OnEmptyCallback?.Invoke();
                }
            }
        }
    }
}