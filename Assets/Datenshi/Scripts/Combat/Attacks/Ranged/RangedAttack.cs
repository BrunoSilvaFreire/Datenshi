using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks.Ranged {
    [CreateAssetMenu(menuName = "Datenshi/Combat/RangedAttack")]
    public class RangedAttack : Attack {
        public static readonly Variable<float> LastFire = new Variable<float>("entity.combat.lastFire", 0);
        public Projectile Prefab;
        public float TimeDelay;
        public Vector2 Offset;
        public bool Aim = true;
        [HideIf("Aim")]
        public bool RawDir = true;
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

        public override void Execute(LivingEntity entity) {
            var lastFire = entity.GetVariable(LastFire);
            var time = Time.time;
            if (!(time - lastFire > TimeDelay)) {
                return;
            }

            var offset = (Vector3) Offset;
            offset.x *= entity.CurrentDirection.X;
            var startPos = entity.transform.position + offset;
            var target = entity.GetVariable(AttackingState.EntityTarget);
            var proj = Prefab.Clone(startPos);
            if (Aim) {
                proj.Shoot(entity, target);
            } else {
                proj.Shoot(entity, RawDir ? entity.CurrentDirection : entity.InputProvider.GetInputVector());
            }
            entity.SetVariable(LastFire, time);
        }
    }
}