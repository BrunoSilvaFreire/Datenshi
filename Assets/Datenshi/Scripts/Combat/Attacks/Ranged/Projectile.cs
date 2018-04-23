using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Interaction;
using Datenshi.Scripts.Misc;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks.Ranged {
    public class Projectile : Ownable, IDefendable {
        [ShowInInspector, ReadOnly]
        private Vector2 velocity;

        public float Speed;
        public uint Damage;

        public void Shoot(LivingEntity shooter, LivingEntity target) {
            Shoot(shooter, target.Hitbox.bounds.center - transform.position);
        }

        public void Shoot(LivingEntity shooter, Vector2 direction) {
            Owner = shooter;
            Owner.OnKilled.AddListener(OnKilled);
            velocity = direction;
            velocity.Normalize();
            velocity *= Speed;
        }

        private void OnKilled() {
            Destroy(gameObject);
        }

        private void Update() {
            if (Owner == null) {
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
            if (e != null && e.Ignored) {
                return;
            }

            if (e != null && e != Owner && e.Relationship != Owner.Relationship) {
                e.Damage(e, Damage);
            }

            if (e == null || e.Relationship != Owner.Relationship) {
                Destroy(gameObject);
            }
        }

        public bool CanDefend(LivingEntity entity) {
            return Owner != null && Owner.Relationship != entity.Relationship;
        }

        public void Defend(LivingEntity entity) {
            velocity = Owner.transform.position - entity.transform.position;
            velocity.Normalize();
            velocity *= GameResources.Instance.DeflectSpeed;

            Owner = entity;
        }

        public bool CanPoorlyDefend(LivingEntity entity) {
            return true;
        }

        public float MaxAngle;

        public void PoorlyDefend(LivingEntity entity) {
            var angle = Random.value * MaxAngle - MaxAngle / 2 + Angle(velocity);
            velocity = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            velocity *= GameResources.Instance.DeflectSpeed;
            Owner = entity;
        }

        public static float Angle(Vector2 vec) {
            if (vec.x < 0) {
                return 360 - Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg * -1;
            }

            return Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;
        }

        public DefenseType GetDefenseType() {
            return DefenseType.Deflect;
        }
    }
}