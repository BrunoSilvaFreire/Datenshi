using System;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UPM.Motors;

namespace Datenshi.Scripts.Entities {
    [Serializable]
    public class EntityDamagedEvent : UnityEvent<ICombatant, uint> { }

    public class GlobalEntityDamagedEvent : UnityEvent<ICombatant, ICombatant, Attack, uint> {
        public static readonly GlobalEntityDamagedEvent Instance = new GlobalEntityDamagedEvent();
        private GlobalEntityDamagedEvent() { }
    }


    [Serializable]
    public class EntityAttackEvent : UnityEvent<Attack> { }

    /// <summary>
    /// Uma entidade que tem vida, vida máxima, e pode ser morta.
    /// </summary>
    public partial class LivingEntity : Entity {
        public const string HealthGroup = "Health";
        public const string CombatGroup = "Combat";
        public static readonly Color HitboxColor = new Color(0.46f, 1f, 0.01f, 0.25f);

        [SerializeField]
        private Collider2D hitbox;

        public Collider2D Hitbox => hitbox;



        [ShowInInspector, ReadOnly, TitleGroup(CombatGroup)]
        protected virtual void Update() {
            UpdateRendering();
            UpdateInvulnerability();
            UpdateStun();
            UpdateFocus();
        }

        private void UpdateRendering() {
            if (ColorizableRenderer != null) {
                ColorizableRenderer.Outline = IsInvulnerable;
            }
        }


        [SerializeField]
        private bool ignored;


        public Direction CurrentDirection {
            get {
                return direction;
            }
            set {
                direction = value;
            }
        }

        public GameObject GameObject => gameObject;

        public bool Ignored {
            get {
                return ignored;
            }
            set {
                ignored = value;
            }
        }


        public override Transform Transform => transform;


        public override Vector2 Center {
            get {
                var m = this as IMovable;
                if (m == null) {
                    return transform.position;
                }

                var hb = m.Hitbox;
                return hb != null ? hb.bounds.center : transform.position;
            }
        }


        public override Vector2 GroundPosition {
            get {
                var pos = Center;
                pos.y -= hitbox.bounds.size.y / 2;
                return pos;
            }
        }

        private void OnDrawGizmos() {
            var b = Hitbox.bounds;
            Gizmos.color = HitboxColor;
            Gizmos.DrawCube(b.center, b.size);
        }
    }
}