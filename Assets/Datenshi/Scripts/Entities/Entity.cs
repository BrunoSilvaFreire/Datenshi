using System;
using System.Collections.Generic;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.World.Rooms;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    [Serializable]
    public sealed class EntityInputProviderChangedEvent : UnityEvent<Entity, DatenshiInputProvider> {
        public static readonly EntityInputProviderChangedEvent Instance = new EntityInputProviderChangedEvent();
        private EntityInputProviderChangedEvent() { }
    }

    [Serializable]
    public sealed class EntityEvent : UnityEvent<Entity> { }

    /// <summary>
    /// Classe base de todas as entidades.
    /// Qualquer objeto que tenha um comportamento no jogo é considerado uma entidade.
    /// </summary>
    public class Entity : MonoBehaviour, IColorizable, IRoomMember, IInputReceiver {
        public static readonly EntityEvent EntityEnabledEvent = new EntityEvent();
        public static readonly EntityEvent EntityDisabledEvent = new EntityEvent();
        public const string MiscGroup = "Misc";
        public const string GeneralGroup = "General";

        public Room Room {
            get;
            private set;
        }

        public bool RequestRoomMembership(Room r) {
            if (Room) {
                return false;
            }
            Room = r;
            return true;
        }

        /// <summary>
        /// O provedor a partir de qual essa entidade está recebendo input.
        /// </summary>
        [TitleGroup(MiscGroup, "Informações que não se encaixam em outros lugares aparecem aqui"), ShowInInspector]
        public DatenshiInputProvider InputProvider => inputProvider;

        [SerializeField, HideInInspector]
        private DatenshiInputProvider inputProvider;

        public bool IsOwned => inputProvider != null;

        public bool RequestOwnership(DatenshiInputProvider provider) {
            if (IsOwned) {
                return false;
            }

            inputProvider = provider;
            return true;
        }


        [TitleGroup(GeneralGroup), SerializeField]
        private ColorizableRenderer colorizableRenderer;

        public ColorizableRenderer ColorizableRenderer => colorizableRenderer;

        public EntityMiscController MiscController;

        [TitleGroup(GeneralGroup)]
        public Character.Character Character;

        [TitleGroup(MiscGroup)]
        protected Direction direction;

       
        
        [SerializeField]
        private UnityEvent onDestroyed;

        public UnityEvent OnDestroyed => onDestroyed;

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


        public void RevokeOwnership() {
            inputProvider = null;
        }

        private void OnEnable() {
            EntityEnabledEvent.Invoke(this);
        }

        private void OnDisable() {
            EntityDisabledEvent.Invoke(this);
        }

        private void OnDrawGizmos() {
            if (InputProvider == null) {
                return;
            }
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

        public string Key => key;

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