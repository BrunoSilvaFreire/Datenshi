using System;
using System.Collections.Generic;
using Datenshi.Scripts.Animation;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Entities.Motors;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Misc;
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

    [Serializable]
    public sealed class EntityEvent : UnityEvent<Entity> { }

    /// <inheritdoc />
    /// <summary>
    /// Classe base de todas as entidades.
    /// Qualquer objeto que tenha um comportamento no jogo é considerado uma entidade.
    /// </summary>
    public class Entity : MonoBehaviour {
        public static readonly EntityEvent EntityEnabledEvent = new EntityEvent();
        public static readonly EntityEvent EntityDisabledEvent = new EntityEvent();
        public const string MiscGroup = "Misc";
        public const string GeneralGroup = "General";

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
        [TitleGroup(GeneralGroup)]
        public Collider2D Hitbox;

        [TitleGroup(GeneralGroup)]
        public EntityAnimatorUpdater AnimatorUpdater;

        [TitleGroup(GeneralGroup)]
        public ColorizableRenderer Renderer;

        [TitleGroup(GeneralGroup)]
        public EntityMiscController MiscController;

        [TitleGroup(GeneralGroup)]
        public Character.Character Character;

        [TitleGroup(MiscGroup)]
        public Direction CurrentDirection;

        public MotorConfig Config;

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

        private void OnEnable() {
            EntityEnabledEvent.Invoke(this);
        }

        private void OnDisable() {
            EntityDisabledEvent.Invoke(this);
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