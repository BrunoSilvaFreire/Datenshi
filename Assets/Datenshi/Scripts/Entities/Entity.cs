using System;
using System.Collections.Generic;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Entities.Motors;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Stealth;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    [Serializable]
    public sealed class EntityInputProviderChangedEvent : UnityEvent<Entity, InputProvider> {
        public static readonly EntityInputProviderChangedEvent Instance = new EntityInputProviderChangedEvent();
        private EntityInputProviderChangedEvent() { }
    }

    /// <inheritdoc />
    /// <summary>
    /// Classe base de todas as entidades.
    /// Qualquer objeto que tenha um comportamento no jogo é considerado uma entidade.
    /// </summary>
    public class Entity : MonoBehaviour {
        public const string MiscGroup = "Misc";

        /// <summary>
        /// O provedor a partir de qual essa entidade está recebendo input.
        /// </summary>
        [TitleGroup(MiscGroup, "Informações que não se encaixam em outros lugares aparecem aqui"), ShowInInspector]
        public InputProvider InputProvider {
            get {
                return inputProvider;
            }
        }

        [SerializeField, HideInInspector]
        private InputProvider inputProvider;

        public bool IsOwned {
            get {
                return inputProvider != null;
            }
        }

        public bool RequestOwnership(InputProvider provider) {
            if (IsOwned) {
                return false;
            }

            inputProvider = provider;
            return true;
        }

        /// <summary>
        /// A hitbox desta entidade
        /// </summary>
        [TitleGroup(MiscGroup)]
        public Collider2D Hitbox;

        public Character.Character Character;

        [TitleGroup(MiscGroup)]
        public Direction CurrentDirection;

        public MotorConfig Config;
        public AbilityController AbilityController;

        [ShowInInspector, ReadOnly, TitleGroup(MiscGroup)]
        private List<VariableValue> variables = new List<VariableValue>();

        public void SetVariable<T>(Variable<T> variable, T value) {
            var key = variable.Key;
            VariableValue instance = null;
            foreach (var variableValue in variables) {
                if (string.Equals(variableValue.Key, key)) {
                    instance = variableValue;
                }
            }

            if (instance == null) {
                instance = new VariableValue(key, value);
                variables.Add(instance);
                return;
            }

            instance.Value = value;
        }

        public T GetVariable<T>(Variable<T> variable) {
            var key = variable.Key;
            foreach (var variableValue in variables) {
                if (string.Equals(variableValue.Key, key)) {
                    return (T) variableValue.Value;
                }
            }

            return variable.DefaultValue;
        }
#if UNITY_EDITOR
        [Button]
        public void SnapToFloor() {
            var bounds = (Bounds2D) Hitbox.bounds;
            var centerX = bounds.Center.x;
            var y = bounds.Min.y;
            var origin = new Vector2(centerX, y);
            var raycast = Physics2D.Raycast(
                origin,
                Vector2.down,
                float.PositiveInfinity,
                GameResources.Instance.WorldMask);
            if (!raycast) {
                return;
            }

            transform.position = raycast.point + new Vector2(0, bounds.Size.y / 2);
        }
#endif
        public void RevokeOwnership() {
            inputProvider = null;
        }
    }

    public sealed class VariableValue {
        [ShowInInspector]
        private readonly string key;

        [ShowInInspector]
        private object value;

        public VariableValue(string key, object value) {
            this.key = key;
            this.value = value;
        }

        public string Key {
            get {
                return key;
            }
        }

        public object Value {
            get {
                return value;
            }
            set {
                this.value = value;
            }
        }
    }
}