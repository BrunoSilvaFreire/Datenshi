using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Combat.Status;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Game.Ranged {
    [CreateAssetMenu(menuName = "Datenshi/Combat/RangedAttack")]
    public class RangedAttack : Attack {
        public static readonly Variable<float> LastFire = new Variable<float>("entity.combat.lastFire", 0);
        public Projectile Prefab;
        public float TimeDelay;
        public Vector2 Offset;
        public bool Aim = true;
        public float FocusConsumption;

        [HideIf("Aim")]
        public bool RawDir = true;

        public uint Damage = 5;
        public float EvasionDashDistance = 3;
        public float EvasionDashDuration = .25F;

        public SpeedStatusEffect SpeedStatusEffect;


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
            var lastFire = entity.GetVariable(LastFire);
            var time = Time.time;
            if (!(time - lastFire > TimeDelay)) {
                return;
            }

            var offset = Offset;
            offset.x *= entity.CurrentDirection.X;
            var startPos = entity.Center + offset;
            var target = entity.GetVariable(CombatVariables.AttackTarget);
            var proj = Prefab.Clone(startPos);
            if (Aim && target != null) {
                proj.Shoot(this, entity, target);
            } else {
                proj.Shoot(this, entity, RawDir ? entity.CurrentDirection : entity.InputProvider.GetInputVector());
            }

            entity.SetVariable(LastFire, time);
        }

        public override uint GetDamage(ICombatant livingEntity) {
            return Damage;
        }
    }
}