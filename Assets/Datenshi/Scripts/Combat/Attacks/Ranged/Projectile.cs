using Datenshi.Scripts.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks.Ranged {
    public class Projectile : MonoBehaviour {
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

        private void OnTriggerEnter2D(Collider2D other) {
            var e = other.GetComponentInParent<LivingEntity>();
            if (e != null && e != owner && e.Relationship != owner.Relationship) {
                e.Damage(e, Damage);
            }

            if (e == null || e.Relationship != owner.Relationship) {
                Destroy(gameObject);
            }
        }
    }
}