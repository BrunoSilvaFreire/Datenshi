using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Entities;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class AssistantTargetSetter : MonoBehaviour {
        [SerializeField, HideInInspector]
        private LivingEntity entity;

        public MovableEntity Assistant;

        [ShowInInspector]
        public LivingEntity Entity {
            get {
                return entity;
            }
            set {
                entity = value;
#if UNITY_EDITOR
                if (EditorApplication.isPlaying) {
                    Assistant.SetVariable(AttackingState.EntityTarget, entity);
                }
#endif
            }
        }

        private void Start() {
            Assistant.SetVariable(AttackingState.EntityTarget, entity);
        }
    }
}