using System;
using System.Collections;
using System.Net;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Combat.Status;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace Datenshi.Scripts.Combat.Game.Ranged {
    [CreateAssetMenu(menuName = "Datenshi/Combat/RangedAttack")]
    public class RangedAttack : Attack {
        public enum DirectionMode {
            Direction,
            Input,
            Attack,
            Constant
        }

        public static readonly Variable<float> LastFire = new Variable<float>("entity.combat.lastFire", 0);
        public Projectile Prefab;
        public float TimeDelay;
        public Vector2 Offset;

        [ShowIf(nameof(IsConstantDirection))]
        public Vector2 ConstantDirection;

        public float Noise;
        public float FocusConsumption;
        public ushort TotalProjectiles;
        public bool OverTime;
        public DirectionMode Direction;
        public bool IsConstantDirection => Direction == DirectionMode.Constant;

        [ShowInInspector]
        public float Duration {
            get {
                return TotalProjectiles * TimeDelay;
            }
            set {
                TimeDelay = value / TotalProjectiles;
            }
        }

        public uint Damage = 5;


#if UNITY_EDITOR
        [ShowInInspector]
        public GameObject CopyOffset {
            get {
                return null;
            }
            set {
                Offset = value.transform.localPosition;
            }
        }
#endif
        [ShowInInspector]
        public float BulletsPerSecond {
            get {
                return 1 / TimeDelay;
            }
            set {
                TimeDelay = 1 / value;
            }
        }


        public override void Execute(ICombatant entity) {
            if (OverTime) {
                entity.StartCoroutine(DoBurst(entity));
            } else {
                for (ushort i = 0; i < TotalProjectiles; i++) {
                    Shoot(entity);
                }
            }
        }

        private IEnumerator DoBurst(ICombatant entity) {
            for (ushort i = 0; i < TotalProjectiles; i++) {
                Shoot(entity);
                yield return new WaitForSeconds(TimeDelay);
            }
        }

        private void Shoot(ICombatant entity) {
            var lastFire = entity.GetVariable(LastFire);
            var time = Time.time;
            if (!(time - lastFire > TimeDelay)) {
                return;
            }

            var offset = Offset;
            offset.x *= entity.CurrentDirection.X;
            var startPos = entity.Center + offset;
            var proj = Prefab.Clone(startPos);
            var dir = GetDirection(entity);
            var angle = Mathf.Atan2(dir.x, dir.y) + UnityEngine.Random.Range(-Noise, Noise);
            var finalDir = new Vector2(
                Mathf.Sin(angle),
                Mathf.Cos(angle)
            );
            proj.Shoot(this, entity, finalDir);

            entity.SetVariable(LastFire, time);
        }

        private Vector2 GetDirection(ICombatant entity) {
            if (Direction == DirectionMode.Attack) {
                var target = entity.GetVariable(CombatVariables.AttackTarget);
                if (target != null) {
                    var dir = entity.Center - target.Center;
                    dir.Normalize();
                    return dir;
                }
            }

            switch (Direction) {
                case DirectionMode.Direction:
                    return entity.CurrentDirection;
                case DirectionMode.Input:
                    return entity.InputProvider.GetLastValidInputVector();
                case DirectionMode.Constant:
                    return ConstantDirection;
            }

            return entity.CurrentDirection;
        }

        public override uint GetDamage(IDamageable damageable) {
            return Damage;
        }
    }
}