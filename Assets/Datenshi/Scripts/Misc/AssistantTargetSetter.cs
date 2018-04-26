using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
                    Assistant.SetVariable(CombatVariables.EntityTarget, entity);
                }
#endif
            }
        }

        private void Start() {
            Assistant.SetVariable(CombatVariables.EntityTarget, entity);
        }
    }
}