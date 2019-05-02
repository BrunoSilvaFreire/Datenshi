using System;
using System.Collections.Generic;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Graphics.Colorization;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Movement;
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
    public class Entity : MonoBehaviour, ILocatable, IColorizable, IRoomMember, IInputReceiver {
        public static readonly EntityEvent EntityEnabledEvent = new EntityEvent();
        public static readonly EntityEvent EntityDisabledEvent = new EntityEvent();
        public const string MiscGroup = "Misc";
        public const string GeneralGroup = "General";


        [BoxGroup(MiscGroup), SerializeField]
        private bool timeScaleIndependent;

        public bool TimeScaleIndependent {
            get {
                return timeScaleIndependent;
            }
            set {
                timeScaleIndependent = value;
            }
        }

        public float DeltaTime => TimeScaleIndependent ? Time.unscaledDeltaTime : Time.deltaTime;

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
        [BoxGroup(MiscGroup), ShowInInspector]
        public DatenshiInputProvider InputProvider
            => overrideInputProvider != null ? overrideInputProvider : inputProvider;

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


        [BoxGroup(GeneralGroup), SerializeField]
        private ColorizableRenderer colorizableRenderer;

        public ColorizableRenderer ColorizableRenderer {
            get {
                return colorizableRenderer;
            }
            protected set {
                colorizableRenderer = value;
            }
        }

        [BoxGroup(GeneralGroup)]
        public EntityMiscController MiscController;

        [BoxGroup(GeneralGroup)]
        public Character.Character Character;

        [BoxGroup(GeneralGroup)]
        protected Direction direction;


        [SerializeField, BoxGroup(MiscGroup, order: 10)]
        private UnityEvent onDestroyed;

        public UnityEvent OnDestroyed => onDestroyed;

        [ShowInInspector, ReadOnly, BoxGroup(MiscGroup)]
        private List<VariableValue> variables = new List<VariableValue>();


        public void SetVariable<T>(Variable<T> variable, T value) {
            var key = (PropertyName) variable.Key;
            VariableValue instance = null;
            foreach (var variableValue in variables) {
                if (variableValue.Key == key) {
                    instance = variableValue;
                }
            }

            if (instance == null) {
                instance = new VariableValue(variable.Key, value);
                variables.Add(instance);
                return;
            }

            instance.Value = value;
        }

        public T GetVariable<T>(Variable<T> variable) {
            var key = (PropertyName) variable.Key;
            foreach (var variableValue in variables) {
                if (variableValue.Key == key) {
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

        public virtual Transform Transform => transform;

        public virtual Vector2 Center => transform.position;

        public virtual Vector2 GroundPosition => Center;

        public void ForceRequestOwnership(DatenshiInputProvider player) {
            if (IsOwned) {
                RevokeOwnership();
            }

            RequestOwnership(player);
        }

        private DatenshiInputProvider overrideInputProvider;

        public void ReleaseOverrideInputProvider() {
            overrideInputProvider = null;
        }

        public void OverrideInputProvider(DummyInputProvider getComponentInChildren) {
            Debug.Log("Overriding input provider @ " + getComponentInChildren);
            overrideInputProvider = getComponentInChildren;
        }
    }

    public sealed class VariableValue {
        [ShowInInspector]
        private readonly PropertyName key;

        [ShowInInspector]
        private object value;

        public VariableValue(string key, object value) {
            this.key = key;
            this.value = value;
        }

        public PropertyName Key => key;

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