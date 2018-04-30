using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Interaction;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Combat.Attacks.Ranged {
    public class ProjectileShotEvent : UnityEvent<Projectile, LivingEntity, LivingEntity> {
        public static readonly ProjectileShotEvent Instance = new ProjectileShotEvent();
        private ProjectileShotEvent() { }
    }

    public class Projectile : Ownable, IDefendable {
        [ShowInInspector, ReadOnly]
        private Vector2 velocity;

        public float Speed;
        public float DestroyDelay = 2;
        public uint Damage;
        public GameObject SpawnOnHit;
        public AudioClip Clip;
        public AudioSource Source;
        public GameObject[] ToDecouple;
        public GameObject OnDefended;
        private bool wasShot;

        public void Shoot(LivingEntity shooter, LivingEntity target) {
            Shoot(shooter, target.Hitbox.bounds.center - transform.position);
            ProjectileShotEvent.Instance.Invoke(this, shooter, target);
        }

        public void Shoot(LivingEntity shooter, Vector2 direction) {
            Owner = shooter;
            wasShot = true;
            velocity = direction;
            velocity.Normalize();
            velocity *= Speed;

            if (Source != null && Clip != null) {
                Source.PlayOneShot(Clip);
            }
        }

        private void Update() {
            if (!wasShot) {
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
            if (e != null) {
                if (e.Ignored) {
                    return;
                }

                if (Owner.ShouldAttack(e)) {
                    e.Damage(e, Damage);
                } else {
                    return;
                }
            }

            Hit();
        }

        private void Hit() {
            foreach (var obj in ToDecouple) {
                obj.transform.parent = null;
                Destroy(obj, DestroyDelay);
            }

            SpawnOnHit.Clone(transform.position);
            Destroy(gameObject);
        }

        public bool CanDefend(LivingEntity entity) {
            return Owner != null && Owner.Relationship != entity.Relationship;
        }

        public void Defend(LivingEntity entity) {
            velocity = Owner.transform.position - entity.transform.position;
            velocity.Normalize();
            velocity *= GameResources.Instance.DeflectSpeed;

            Owner = entity;
            if (OnDefended != null) {
                OnDefended.Clone(transform.position);
            }
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