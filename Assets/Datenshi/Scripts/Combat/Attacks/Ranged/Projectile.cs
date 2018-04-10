using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Interaction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks.Ranged {
    public class Projectile : Defendable {
        [ShowInInspector, ReadOnly]
        private LivingEntity owner;

        [ShowInInspector, ReadOnly]
        private Vector2 velocity;

        public float Speed;
        public uint Damage;

        public void Shoot(LivingEntity shooter, LivingEntity target) {
            owner = shooter;
            velocity = target.transform.position - transform.position;
            velocity.Normalize();
            velocity *= Speed;
        }

        private void Update() {
            if (owner == null) {
                return;
            }

            transform.position += (Vector3) velocity * Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            var col = other.collider;
            if (col.GetComponent<Projectile>() != null) {
                return;
            }

            var e = col.GetComponentInParent<LivingEntity>();
            if (e != null && e != owner && e.Relationship != owner.Relationship) {
                e.Damage(e, Damage);
            }

            if (e == null || e.Relationship != owner.Relationship) {
                Destroy(gameObject);
            }
        }

        public override bool CanDefend(LivingEntity entity) {
            return owner != null && owner.Relationship != entity.Relationship;
        }

        public override void Defend(LivingEntity entity) {
            velocity = owner.transform.position - entity.transform.position;
            velocity.Normalize();
            velocity *= GameResources.Instance.DeflectSpeed;

            owner = entity;
        }

        public override bool CanPoorlyDefend(LivingEntity entity) {
            return true;
        }

        public float MaxAngle;

        public override void PoorlyDefend(LivingEntity entity) {
            var angle = Random.value * MaxAngle - MaxAngle / 2 + Angle(velocity);
            velocity = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            velocity *= GameResources.Instance.DeflectSpeed;
            owner = entity;
        }

        public static float Angle(Vector2 vec) {
            if (vec.x < 0) {
                return 360 - Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg * -1;
            }

            return Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;
        }

        public override DefenseType GetDefenseType() {
            return DefenseType.Deflect;
        }
    }
}