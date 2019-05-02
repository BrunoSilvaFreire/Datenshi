﻿using System;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Buffs;
using Shiroi.FX.Services;
using Shiroi.FX.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities {
    [Serializable]
    public class EntityDamagedEvent : UnityEvent<IDamageDealer, uint> { }

    public class GlobalEntityDamagedEvent : UnityEvent<LivingEntity, IDamageDealer, IDamageSource, uint> {
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
        public static readonly Color HitboxColor = new Color(0.46f, 1f, 0.01f, 0.25f);

        [SerializeField, BoxGroup(GeneralGroup)]
        private Collider2D hitbox;

        public Collider2D Hitbox => hitbox;

        protected virtual void Start() {
            CurrentDirection = Direction.Right;
            InitDefense();
        }

        protected virtual void Update() {
            UpdateRendering();
            UpdateInvulnerability();
            UpdateStun();
            UpdateStamina();
            UpdateDefense();
            damageMultiplier.Tick();

/*            if (AnimatorUpdater != null) {
                AnimatorUpdater.UpdateAnimator();
            }*/
        }

        [BoxGroup(MiscGroup)]
        public float OutlineInvulnerabilityMinSecondsLeft = 2;

        private void UpdateRendering() { }


        [SerializeField, BoxGroup(GeneralGroup)]
        private bool ignored;


        public Direction CurrentDirection {
            get { return direction; }

            set { direction = value; }
        }

        public GameObject GameObject => gameObject;

        public bool Ignored {
            get { return ignored; }

            set { ignored = value; }
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

        protected virtual void OnDrawGizmos() {
            var b = Hitbox.bounds;
            Gizmos.color = HitboxColor;
            Gizmos.DrawCube(b.center, b.size);
        }
    }
}